using System.ComponentModel;
using JetBrains.Annotations;
using RobloxCS.Common;
using RobloxCS.Packages;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Remove;

[Description("Remove a dependency from the project")]
[UsedImplicitly]
public sealed class PackageRemoveCommand : AsyncCommand<PackageRemoveSettings> {
    private readonly IAnsiConsole _console;

    public PackageRemoveCommand(IAnsiConsole console) {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, PackageRemoveSettings settings, CancellationToken cancellationToken) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);

        var root = Path.GetFullPath(settings.ProjectPath ?? Environment.CurrentDirectory);
        var name = settings.Package.Trim().ToLowerInvariant();

        try {
            var manifest = ProjectManifest.Load(root);

            if (!manifest.Dependencies.ContainsKey(name)) {
                throw new PackageException($"'{name}' is not a dependency of this project");
            }

            ManifestEditor.RemoveDependency(root, name);

            var dependencies = ProjectManifest.Load(root).Dependencies;
            var resolver = new Resolver(root, TypesInstall.Root, manifest.Sources, manifest.RobloxCSVersion, Lockfile.Load(root));
            var packages = await resolver.ResolveAllAsync(dependencies, cancellationToken);
            var pruned = PackageInstaller.Apply(root, manifest.RobloxCSVersion, packages);

            _console.MarkupLine($"[green]removed[/] {Markup.Escape(name)}");

            if (pruned > 1) {
                _console.MarkupLine($"[grey]+ {pruned - 1} transitive {(pruned == 2 ? "dependency" : "dependencies")} no longer needed[/]");
            }

            return 0;
        } catch (Exception ex) when (ex is ManifestException or PackageException) {
            _console.MarkupLine($"[red]error:[/] {Markup.Escape(ex.Message)}");

            return -1;
        }
    }
}