using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
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