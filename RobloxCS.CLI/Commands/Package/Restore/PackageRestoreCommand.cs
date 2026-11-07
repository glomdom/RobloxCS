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

        var root = Path.GetFullPath(settings.ProjectPath ?? Environment.CurrentDirectory);

        try {
            var manifest = ProjectManifest.Load(root);

            TypesInstall.EnsureInstalled();

            var packages = await PackageInstaller.RestoreAsync(root, TypesInstall.Root, manifest, cancellationToken);

            _console.MarkupLine($"[green]restored[/] {packages.Count} {(packages.Count == 1 ? "package" : "packages")}");

            return 0;
        } catch (Exception ex) when (ex is ManifestException or PackageException) {
            _console.MarkupLine($"[red]error:[/] {Markup.Escape(ex.Message)}");

            return -1;
        }
    }
}