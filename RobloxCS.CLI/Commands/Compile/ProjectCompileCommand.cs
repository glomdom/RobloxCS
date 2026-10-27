using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using RobloxCS.Common;
using RobloxCS.CompilerPipeline;
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
        var openDiagnostics = new List<Diagnostic>();
        var opened = await ProjectSession.OpenAsync(slnFile, projectFile, options, openDiagnostics, cancellation);

        DiagnosticMarkup.PrintAll(_console, openDiagnostics);

        if (!opened.Ok) {
            return Fail(opened.Diagnostic);
        }

        using var session = opened.Value;

        if (openDiagnostics.Any(d => d.IsError)) {
            return -1;
        }

        var failures = 0;

        await foreach (var result in session.CompileAllAsync(cancellation)) {
            DiagnosticMarkup.PrintAll(_console, result.Diagnostics);

            if (!result.Output.Ok) {
                DiagnosticMarkup.Print(_console, result.Output.Diagnostic);

                failures++;
            }
        }

        return failures == 0 ? 0 : -1;
    }

    private int Fail(Diagnostic diag) {
        DiagnosticMarkup.Print(_console, diag);

        return -1;
    }
}