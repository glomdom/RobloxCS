using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using RobloxCS.Common;
using RobloxCS.Common.Rojo;
using RobloxCS.CompilerPipeline;
using RobloxCS.CompilerPipeline.Documents;
using RobloxCS.CompilerPipeline.Modules;
using RobloxCS.CompilerPipeline.Projects;
using RobloxCS.CompilerPipeline.Solutions;
using Serilog;
using Serilog.Events;
using Spectre.Console;
using Spectre.Console.Cli;
using Diagnostic = RobloxCS.Common.Diagnostics.Diagnostic;

namespace RobloxCS.CLI.Commands.Compile;

[Description("Compile a C# project.")]
[UsedImplicitly]
public sealed class ProjectCompileCommand : AsyncCommand<ProjectCompileCommand.Settings> {
    private readonly IAnsiConsole _console;

    public ProjectCompileCommand(IAnsiConsole console) {
        _console = console;
    }

    public sealed class Settings : CompileSettings {
        [CommandOption("--project")]
        [Description("File path to Rojo's project JSON")]
        public string ProjectJsonPath { get; init; } = "default.project.json";
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation) {
        LoggerSetup.LevelSwitch.MinimumLevel = settings.Verbosity ? LogEventLevel.Verbose : LogEventLevel.Warning;

        var cwd = Environment.CurrentDirectory;
        var discovery = SolutionDiscovery.Discover(cwd);
        if (!discovery.Ok) {
            return Fail(discovery.Diagnostic);
        }

        var slnFile = discovery.Value;
        var projectFile = Path.Combine(cwd, settings.ProjectJsonPath);
        if (!File.Exists(projectFile)) {
            Log.Error("Failed to find {ProjectFileName} in {SearchDirectory}.", settings.ProjectJsonPath, cwd);

            return -1;
        }

        MsBuildRegister.RegisterDefaults();

        return await RunAsync(slnFile, projectFile, new EmitOptions(settings.PrintHir), cancellation);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<int> RunAsync(string slnFile, string projectFile, EmitOptions options, CancellationToken cancellation) {
        var loaded = await SolutionLoader.LoadAsync(slnFile, cancellation);
        if (!loaded.Ok) {
            return Fail(loaded.Diagnostic);
        }

        Log.Debug("Loaded solution from {SolutionFile}", slnFile);

        var loadedAnchors = RojoProject.LoadAnchors(projectFile);
        if (!loadedAnchors.Ok) {
            return Fail(loadedAnchors.Diagnostic);
        }

        Log.Debug("Loaded anchors from rojo project {ProjectFile}", projectFile);

        var solution = loaded.Value;
        var anchors = loadedAnchors.Value;
        var outputs = new List<CompiledFile>();

        var plans = new List<ProjectPlan>();

        foreach (var project in solution.Solution.Projects) {
            var prepared = await ProjectPreparation.PrepareProjectAsync(project, anchors, cancellation);
            if (!prepared.Ok) {
                return Fail(prepared.Diagnostic);
            }

            plans.Add(prepared.Value);

            Log.Debug("Prepared project {ProjectName}", prepared.Value.Project.Name);
        }

        var graphDiagnostics = new List<Diagnostic>();
        var graph = await ModuleGraphBuilder.BuildAsync(plans, graphDiagnostics, cancellation);

        DiagnosticMarkup.PrintAll(_console, graphDiagnostics);

        if (graphDiagnostics.Any(d => d.IsError)) {
            return -1;
        }

        var emitOptions = options with { Modules = graph };

        foreach (var plan in plans) {
            foreach (var document in plan.Project.Documents.Where(Filters.ShouldCompile)) {
                var diagnostics = new List<Diagnostic>();
                var compiled = await DocumentCompiler.CompileDocument(document, plan, emitOptions, diagnostics, cancellation);

                DiagnosticMarkup.PrintAll(_console, diagnostics);

                if (!compiled.Ok) {
                    return Fail(compiled.Diagnostic);
                }

                Log.Debug("Compiled file {FileName}", document.Name);

                outputs.Add(compiled.Value);
            }
        }

        foreach (var file in outputs) {
            Directory.CreateDirectory(Path.GetDirectoryName(file.OutPath)!);
            await File.WriteAllTextAsync(file.OutPath, file.Code, cancellation);
        }

        return 0;
    }

    private int Fail(Diagnostic diag) {
        DiagnosticMarkup.Print(_console, diag);

        return -1;
    }
}