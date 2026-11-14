using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using RobloxCS.CLI.Reporting;
using RobloxCS.Common;
using RobloxCS.CompilerPipeline;
using RobloxCS.Packages;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Build;

[Description("Compile the C# solution in the current directory to Luau.")]
[UsedImplicitly]
public sealed class BuildCommand : AsyncCommand<BuildSettings> {
    private readonly IAnsiConsole _console;

    private static readonly string[] MaybeStaleFiles = [ProjectManifest.FileName, Lockfile.FileName];

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

        var root = located.Value.Root;
        var reporter = new BuildReporter(_console, root);

        ProjectManifest manifest;

        try {
            manifest = ProjectManifest.Load(root);
        } catch (ManifestException ex) {
            new TerminalOutput(_console).Write("error", "red", Markup.Escape(ex.Message));

            return -1;
        }

        if (!CheckTypes(manifest)) {
            return -1;
        }

        try {
            if (PackageInstaller.NeedsRestore(root)) {
                await reporter.RunAsync("restore", "Restoring packages", _ => PackageInstaller.RestoreAsync(root, TypesInstall.Root, manifest, cancellation));
            } else if (PropsAreStale(root)) {
                reporter.Status("Updating project references");

                var packages = Lockfile.Load(root).Packages.Select(package => Path.GetFullPath(Path.Combine(root, package.Project)));

                Props.Write(root, manifest.RobloxCSVersion, packages);
                PackageInstaller.InvalidateRestore(root);
            }
        } catch (PackageException ex) {
            new TerminalOutput(_console).Write("error", "red", Markup.Escape(ex.Message));

            return -1;
        }

        MsBuildRegister.RegisterDefaults();

        return await RunAsync(located.Value, reporter, settings, cancellation);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<int> RunAsync(ProjectLocation location, BuildReporter reporter, BuildSettings settings, CancellationToken cancellation) {
        var options = new EmitOptions(settings.PrintHir, settings.SkipDiagnostics);

        if (NeedsDotnetRestore(location.Root)) {
            if (!await DotnetRestore.RunAsync(_console, location.SolutionFile, cancellation)) {
                return -1;
            }
        }

        if (settings.Watch) {
            return await new WatchLoop(reporter, location, options).RunAsync(cancellation);
        }

        var outcome = await ProjectBuild.OpenAndCompileAsync(reporter, location, options, cancellation);
        outcome.Session?.Dispose();

        return outcome.Ok ? 0 : -1;
    }

    private bool CheckTypes(ProjectManifest manifest) {
        var output = new TerminalOutput(_console);
        var projectVersion = manifest.RobloxCSVersion;

        if (projectVersion != TypesInstall.Version) {
            output.Write("error", "red",
                $"This project targets RobloxCS [white]{Markup.Escape(projectVersion)}[/] " +
                $"but this is rbxcsc [white]{Markup.Escape(TypesInstall.Version)}[/]"
            );

            output.Write("note", "cyan", $"Update [white]robloxcs[/] in {ProjectManifest.FileName}, or switch rbxcsc with [white]rokit[/]");

            return false;
        }

        TypesInstall.EnsureInstalled();

        var typesPath = Path.Combine(TypesInstall.Root, projectVersion, "RobloxCS.Types.dll");

        if (!File.Exists(typesPath)) {
            output.Write("error", "red", $"RobloxCS.Types is missing at [white]{Markup.Escape(typesPath)}[/]");

            return false;
        }

        return true;
    }

    private static bool NeedsDotnetRestore(string root) {
        return Directory
            .EnumerateFiles(root, "*.csproj", SearchOption.AllDirectories)
            .Any(project => !File.Exists(Path.Combine(Path.GetDirectoryName(project)!, "obj", "project.assets.json")));
    }

    private static bool PropsAreStale(string root) {
        var props = Path.Combine(root, "rbxcs.props");
        if (!File.Exists(props)) {
            return true;
        }

        var written = File.GetLastWriteTimeUtc(props);

        return MaybeStaleFiles
            .Select(name => Path.Combine(root, name))
            .Any(path => File.Exists(path) && File.GetLastWriteTimeUtc(path) > written);
    }
}