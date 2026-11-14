using System.ComponentModel;
using JetBrains.Annotations;
using RobloxCS.Common;
using RobloxCS.Packages;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Restore;

[Description("Install dependencies from the lockfile")]
[UsedImplicitly]
public sealed class PackageRestoreCommand : AsyncCommand<PackageSettings> {
    private readonly IAnsiConsole _console;

    public PackageRestoreCommand(IAnsiConsole console) {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, PackageSettings settings, CancellationToken cancellationToken) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);
        var output = new TerminalOutput(_console);

        var root = Path.GetFullPath(settings.ProjectPath ?? Environment.CurrentDirectory);

        try {
            var manifest = ProjectManifest.Load(root);

            TypesInstall.EnsureInstalled();

            var packages = await output.RunAsync("restore", "Restoring packages", _ => PackageInstaller.RestoreAsync(root, TypesInstall.Root, manifest, cancellationToken));

            output.Write("success", "green", $"Restored {packages.Count} {(packages.Count == 1 ? "package" : "packages")}");

            return 0;
        } catch (Exception ex) when (ex is ManifestException or PackageException) {
            output.Write("error", "red", Markup.Escape(ex.Message));

            return -1;
        }
    }
}