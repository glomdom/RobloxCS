using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using RobloxCS.CLI.Reporting;
using RobloxCS.Common;
using RobloxCS.CompilerPipeline;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Build;

[Description("Compile the C# solution in the current directory to Luau.")]
[UsedImplicitly]
public sealed class BuildCommand : AsyncCommand<BuildSettings> {
    private readonly IAnsiConsole _console;

    public BuildCommand(IAnsiConsole console) {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, BuildSettings settings, CancellationToken cancellation) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);

        var located = ProjectLocator.Locate(Environment.CurrentDirectory, settings.ProjectJsonPath);
        if (!located.Ok) {
            DiagnosticMarkup.Print(_console, located.Diagnostic);

            return -1;
        }

        MsBuildRegister.RegisterDefaults();

        return await RunAsync(located.Value, settings, cancellation);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<int> RunAsync(ProjectLocation location, BuildSettings settings, CancellationToken cancellation) {
        var reporter = new BuildReporter(_console, location.Root);
        var options = new EmitOptions(settings.PrintHir, settings.SkipDiagnostics);

        if (settings.Watch) {
            return await new WatchLoop(reporter, location, options).RunAsync(cancellation);
        }

        var outcome = await ProjectBuild.OpenAndCompileAsync(reporter, location, options, null, cancellation);

        outcome.Session?.Dispose();

        return outcome.Ok ? 0 : -1;
    }
}