using System.ComponentModel;
using System.Diagnostics;
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

        if (!CheckTypes(located.Value.Root)) {
            return -1;
        }

        MsBuildRegister.RegisterDefaults();

        return await RunAsync(located.Value, settings, cancellation);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<int> RunAsync(ProjectLocation location, BuildSettings settings, CancellationToken cancellation) {
        var reporter = new BuildReporter(_console, location.Root);
        var options = new EmitOptions(settings.PrintHir, settings.SkipDiagnostics);

        if (NeedsRestore(location.Root)) {
            reporter.Status("restoring");

            if (!await RestoreAsync(location.SolutionFile, cancellation)) {
                reporter.Status("[red]dotnet restore failed[/], run it yourself to see why");

                return -1;
            }
        }

        if (settings.Watch) {
            return await new WatchLoop(reporter, location, options).RunAsync(cancellation);
        }

        var outcome = await ProjectBuild.OpenAndCompileAsync(reporter, location, options, null, cancellation);

        outcome.Session?.Dispose();

        return outcome.Ok ? 0 : -1;
    }
 
    private static bool NeedsRestore(string root) {
        return Directory
            .EnumerateFiles(root, "*.csproj", SearchOption.AllDirectories)
            .Any(project => !File.Exists(Path.Combine(Path.GetDirectoryName(project)!, "obj", "project.assets.json")));
    }
    
    private bool CheckTypes(string root) {
        TypesInstall.EnsureInstalled();

        var projectVersion = TypesInstall.FindProjectVersion(root);
        if (projectVersion is null) {
            _console.MarkupLine("[yellow]warning:[/] no [white]<RobloxCSVersion>[/] found in Directory.Build.props or any csproj, the version can't be checked");

            return true;
        }

        if (projectVersion != TypesInstall.Version) {
            _console.MarkupLine(
                $"[red]error:[/] this project targets RobloxCS [white]{Markup.Escape(projectVersion)}[/] " +
                $"but this is rbxcsc [white]{Markup.Escape(TypesInstall.Version)}[/]"
            );

            _console.MarkupLine("[cyan]note:[/] update [white]<RobloxCSVersion>[/] in Directory.Build.props, or switch rbxcsc with [white]rokit[/]");

            return false;
        }

        var typesPath = Path.Combine(TypesInstall.Root, projectVersion, "RobloxCS.Types.dll");

        if (!File.Exists(typesPath)) {
            _console.MarkupLine($"[red]error:[/] RobloxCS.Types is missing at [white]{Markup.Escape(typesPath)}[/]");

            return false;
        }

        return true;
    }

    private static async Task<bool> RestoreAsync(string solution, CancellationToken cancellation) {
        using var process = Process.Start(new ProcessStartInfo("dotnet", ["restore", solution]) {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        });

        if (process is null) {
            return false;
        }

        await process.WaitForExitAsync(cancellation);

        return process.ExitCode == 0;
    }
}