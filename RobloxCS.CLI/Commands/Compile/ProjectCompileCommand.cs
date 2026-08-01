using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.FileSystemGlobbing;
using RobloxCS.Common;
using RobloxCS.Common.Rojo;
using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;
using Serilog;
using Serilog.Events;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Compile;

[Description("Compile a C# project.")]
[UsedImplicitly]
public sealed class ProjectCompileCommand : AsyncCommand<ProjectCompileCommand.Settings> {
    private const string DefaultProjectFileName = "default.project.json";

    private readonly IAnsiConsole _console;

    public ProjectCompileCommand(IAnsiConsole console) {
        _console = console;
    }

    public sealed class Settings : CompileSettings {
        // [CommandArgument(0, "<path>")]
        // [Description("The path of the [green]C#[/] file to compile.")]
        // public required string Path { get; set; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation) {
        LoggerSetup.LevelSwitch.MinimumLevel = settings.Verbosity ? LogEventLevel.Verbose : LogEventLevel.Warning;

        var fullCwd = Path.GetFullPath(Environment.CurrentDirectory);
        Log.Debug("Searching for candidate .slnx files in {SearchDirectory}", fullCwd);

        var candidateMatcher = new Matcher().AddInclude("*.slnx");
        var slnCandidates = candidateMatcher.GetResultsInFullPath(fullCwd).ToList();

        if (slnCandidates.Count == 0) {
            Log.Error("Failed to find a .slnx file in {SearchDirectory}.", fullCwd);

            return -1;
        }

        var slnFile = slnCandidates.First();

        var projectFile = Path.Combine(fullCwd, DefaultProjectFileName);
        if (!File.Exists(projectFile)) {
            Log.Error("Failed to find {ProjectFileName} in {SearchDirectory}. Output paths are read from it.", DefaultProjectFileName, fullCwd);

            return -1;
        }

        if (!MSBuildLocator.IsRegistered) {
            var vsi = MSBuildLocator.RegisterDefaults();

            Log.Debug("Using dotnet {Version} in {DotnetPath}", vsi.Version, vsi.VisualStudioRootPath);
            Log.Debug("Found MSBuild executable in {ExecutablePath}", vsi.MSBuildPath);
        }

        return await RunAsync(slnFile, projectFile, cancellation);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<int> RunAsync(string slnFile, string projectFile, CancellationToken cancellation) {
        Log.Information("Starting project handling for {ProjectFilePath}", slnFile);

        using var workspace = MSBuildWorkspace.Create();
        workspace.LoadMetadataForReferencedProjects = true;

        var workspaceFailed = false;
        workspace.RegisterWorkspaceFailedHandler((e) => {
            workspaceFailed |= e.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure;

            Log.Error("Workspace {Kind}: {Message}", e.Diagnostic.Kind, e.Diagnostic.Message);
        });

        Log.Debug("Created MSBuild workspace");

        var solution = await workspace.OpenSolutionAsync(slnFile, cancellationToken: cancellation);
        var solutionName = Path.GetFileNameWithoutExtension(slnFile);

        if (workspaceFailed) {
            Log.Error(
                "Solution {SolutionName} did not load cleanly. If the errors above mention unresolved packages or a missing assets file, run `dotnet restore` on it.",
                solutionName
            );

            return -1;
        }

        Log.Verbose("Opened solution {SolutionName}", solutionName);

        var anchors = RojoProject.ReadAnchors(projectFile);
        foreach (var x in anchors) {
            Log.Debug("Got instance anchor {Anchor}", x);
        }

        if (anchors.Count == 0) {
            Log.Error("{ProjectFile} declares no $path entries, so there is nowhere to write output.", projectFile);

            return -1;
        }

        foreach (var project in solution.Projects) {
            var compilation = await GetProjectCompilationAsync(project, cancellation);
            if (compilation is null) return -1;

            if (!compilation.References.Any()) {
                Log.Error(
                    "Project {ProjectName} compiled with zero metadata references. MSBuildWorkspace does not restore, run `dotnet restore` on the target solution.",
                    project.Name
                );

                return -1;
            }

            Log.Debug("Got C# compilation for {ProjectName} with {ReferenceCount} reference(s)", project.Name, compilation.References.Count());

            if (!RojoProject.TryResolveAnchor(project.Name, anchors, out var anchor)) return -1;

            Log.Debug("Mapped {ProjectName} to {Anchor}", project.Name, anchor);

            foreach (var document in project.Documents) {
                if (document.Folders is ["obj", ..]) {
                    Log.Verbose("Skipping {DocumentName} as it is inside intermediates folder", document.Name);

                    continue;
                }

                var syntaxTree = await document.GetSyntaxTreeAsync(cancellation);
                if (syntaxTree is null) {
                    Log.Error("Failed to get syntax tree for document {DocumentName}", document.Name);

                    return -1;
                }

                var compiler = new CSharpCompiler(syntaxTree, compilation);

                foreach (var diag in compiler.FormatDiagnostics()) {
                    _console.MarkupLine(diag);
                }

                if (compiler.HasErrors) {
                    Log.Error("Refusing to transpile {DocumentName}: the C# above did not compile.", document.Name);

                    return 1;
                }

                var scriptType = document.Name switch {
                    var n when n.EndsWith(".server.cs") => ScriptType.Server,
                    var n when n.EndsWith(".client.cs") => ScriptType.Local,
                    _ => ScriptType.Module,
                };

                Log.Verbose(
                    "Compiling {DocumentName} as a {ScriptType} script with folders {Folders}",
                    document.Name, scriptType, document.Folders);

                var transpiler = new CSharpTranspiler(new TranspilerOptions(scriptType), compiler);
                var chunk = transpiler.Transpile();

                var renderer = new RendererWalker();
                var code = renderer.Render(chunk);

                var filename = Path.GetFileNameWithoutExtension(syntaxTree.FilePath);
                var combinedOutDir = Path.Combine([anchor.FullPath, .. document.Folders]);
                var outPath = Path.Combine(combinedOutDir, $"{filename}.luau");

                Directory.CreateDirectory(combinedOutDir);
                Log.Verbose("Writing output to {OutFilePath}", outPath);

                await File.WriteAllTextAsync(outPath, code, cancellation);
            }
        }

        return 0;
    }

    private static async Task<Compilation?> GetProjectCompilationAsync(Project project, CancellationToken cancellation) {
        var compilation = await project.GetCompilationAsync(cancellation);
        if (compilation is not null) return compilation;

        Log.Error("Failed to get compilation from MSBuild for {ProjectName}.", project.Name);

        return null;
    }
}