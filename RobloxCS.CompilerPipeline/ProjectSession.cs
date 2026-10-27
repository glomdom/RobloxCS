using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RobloxCS.Common;
using RobloxCS.Common.Rojo;
using RobloxCS.CompilerPipeline.Documents;
using RobloxCS.CompilerPipeline.Modules;
using RobloxCS.CompilerPipeline.Projects;
using RobloxCS.CompilerPipeline.Solutions;
using RobloxCS.Modules;
using Diagnostic = RobloxCS.Common.Diagnostics.Diagnostic;

namespace RobloxCS.CompilerPipeline;

public sealed record DocumentResult(Document Document, Result<CompiledFile> Output, List<Diagnostic> Diagnostics);

public sealed record SessionUpdate(List<DocumentResult> Results, List<string> DeletedOutputs, List<Diagnostic> Diagnostics, Diagnostic? Failure);

public sealed class ProjectSession : IDisposable {
    private readonly LoadedSolution _loaded;
    private readonly List<RojoAnchor> _anchors;

    public Solution Solution { get; private set; }
    public IReadOnlyList<ProjectPlan> Plans { get; private set; } = [];
    public ModuleGraph Graph { get; private set; } = ModuleGraph.Empty;

    private EmitOptions Options => field with { Modules = Graph };

    private ProjectSession(LoadedSolution loaded, List<RojoAnchor> anchors, EmitOptions options) {
        _loaded = loaded;
        _anchors = anchors;
        Options = options;

        Solution = loaded.Solution;
    }

    public static async Task<Result<ProjectSession>> OpenAsync(
        string slnFile,
        string projectFile,
        EmitOptions options,
        List<Diagnostic> diagnostics,
        CancellationToken cancellation
    ) {
        var loaded = await SolutionLoader.LoadAsync(slnFile, cancellation);
        if (!loaded.Ok) {
            return loaded.Diagnostic;
        }

        var anchors = RojoProject.LoadAnchors(projectFile);
        if (!anchors.Ok) {
            loaded.Value.Dispose();

            return anchors.Diagnostic;
        }

        var session = new ProjectSession(loaded.Value, anchors.Value, options);

        if (await session.RefreshAsync(diagnostics, cancellation) is { } failure) {
            session.Dispose();

            return failure;
        }

        return session;
    }

    public async IAsyncEnumerable<DocumentResult> CompileAllAsync([EnumeratorCancellation] CancellationToken cancellation) {
        foreach (var plan in Plans) {
            foreach (var document in plan.Project.Documents.Where(Filters.ShouldCompile)) {
                yield return await CompileAsync(plan, document, cancellation);
            }
        }
    }

    public async Task<DocumentResult> CompileAsync(ProjectPlan plan, Document document, CancellationToken cancellation) {
        var diagnostics = new List<Diagnostic>();
        var output = await DocumentCompiler.CompileDocument(document, plan, Options, diagnostics, cancellation);

        if (output.Ok) {
            Directory.CreateDirectory(Path.GetDirectoryName(output.Value.OutPath)!);
            await File.WriteAllTextAsync(output.Value.OutPath, output.Value.Code, cancellation);
        }

        return new DocumentResult(document, output, diagnostics);
    }

    public async Task<SessionUpdate> ApplyChangeAsync(IReadOnlyCollection<string> paths, CancellationToken cancellation) {
        var changed = new HashSet<DocumentId>();
        var deletedOutputs = new List<string>();
        var oldGraph = Graph;
        var solution = Solution;

        foreach (var path in paths) {
            var ids = solution.GetDocumentIdsWithFilePath(path);

            if (!File.Exists(path)) {
                foreach (var id in ids) {
                    if (solution.GetDocument(id) is { } document && PlanFor(id.ProjectId) is { } plan) {
                        deletedOutputs.Add(DocumentCompiler.OutPathFor(document, plan));
                    }

                    solution = solution.RemoveDocument(id);
                }

                continue;
            }

            var text = SourceText.From(await ReadWithRetryAsync(path, cancellation));

            if (ids.Length > 0) {
                foreach (var id in ids) {
                    if (solution.GetDocument(id) is { } existing && (await existing.GetTextAsync(cancellation)).ContentEquals(text)) {
                        continue;
                    }

                    solution = solution.WithDocumentText(id, text);
                    changed.Add(id);
                }

                continue;
            }

            if (OwningProject(solution, path) is { } project) {
                var id = DocumentId.CreateNewId(project.Id);
                var folders = FoldersOf(project, path);

                solution = solution.AddDocument(id, Path.GetFileName(path), text, folders, path);
                changed.Add(id);
            }
        }

        Solution = solution;

        var diagnostics = new List<Diagnostic>();
        if (await RefreshAsync(diagnostics, cancellation) is { } failure) {
            return new SessionUpdate([], deletedOutputs, diagnostics, failure);
        }

        foreach (var output in deletedOutputs) {
            File.Delete(output);
        }

        var toCompile = new HashSet<DocumentId>(changed);

        if (GraphChanged(oldGraph, Graph)) {
            var dependencyGraph = Solution.GetProjectDependencyGraph();
            var touchedProjects = changed.Select(id => id.ProjectId).Distinct().ToList();

            var affected = touchedProjects
                .Concat(touchedProjects.SelectMany(dependencyGraph.GetProjectsThatTransitivelyDependOnThisProject))
                .ToHashSet();

            foreach (var projectId in affected) {
                toCompile.UnionWith(Solution.GetProject(projectId)!.DocumentIds);
            }
        }

        var results = new List<DocumentResult>();

        foreach (var id in toCompile) {
            if (Solution.GetDocument(id) is not { } document || !Filters.ShouldCompile(document) || PlanFor(id.ProjectId) is not { } plan) {
                continue;
            }

            results.Add(await CompileAsync(plan, document, cancellation));
        }

        return new SessionUpdate(results, deletedOutputs, diagnostics, null);
    }

    private ProjectPlan? PlanFor(ProjectId id) => Plans.FirstOrDefault(plan => plan.Project.Id == id);

    private static Project? OwningProject(Solution solution, string path) {
        return solution.Projects
            .Where(project => project.FilePath is not null)
            .Select(project => (Project: project, Directory: Path.GetDirectoryName(project.FilePath)! + Path.DirectorySeparatorChar))
            .Where(entry => path.StartsWith(entry.Directory, StringComparison.OrdinalIgnoreCase))
            .MaxBy(entry => entry.Directory.Length)
            .Project;
    }

    private static string[] FoldersOf(Project project, string path) {
        var relative = Path.GetRelativePath(Path.GetDirectoryName(project.FilePath)!, Path.GetDirectoryName(path)!);

        return relative == "." ? [] : relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    private static bool GraphChanged(ModuleGraph before, ModuleGraph after) {
        if (before.Types.Count != after.Types.Count) {
            return true;
        }

        foreach (var (key, location) in after.Types) {
            if (!before.Types.TryGetValue(key, out var old) || old.IsScript != location.IsScript || !old.InstancePath.SequenceEqual(location.InstancePath)) {
                return true;
            }
        }

        return false;
    }

    private static async Task<string> ReadWithRetryAsync(string path, CancellationToken cancellation) {
        for (var attempt = 0;; attempt++) {
            try {
                return await File.ReadAllTextAsync(path, cancellation);
            } catch (IOException) when (attempt < 5) {
                await Task.Delay(50, cancellation);
            }
        }
    }

    private async Task<Diagnostic?> RefreshAsync(List<Diagnostic> diagnostics, CancellationToken cancellation) {
        var plans = new List<ProjectPlan>();

        foreach (var project in Solution.Projects) {
            var prepared = await ProjectPreparation.PrepareProjectAsync(project, _anchors, cancellation);
            if (!prepared.Ok) {
                return prepared.Diagnostic;
            }

            plans.Add(prepared.Value);
        }

        Plans = plans;
        Graph = await ModuleGraphBuilder.BuildAsync(plans, diagnostics, cancellation);

        return null;
    }

    public void Dispose() {
        _loaded.Dispose();
    }
}