using System.ComponentModel;
using JetBrains.Annotations;
using RobloxCS.Common;
using RobloxCS.Packages;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Add;

[Description("Add a dependency to the project")]
[UsedImplicitly]
public sealed class PackageAddCommand : AsyncCommand<PackageAddSettings> {
    private readonly IAnsiConsole _console;

    public PackageAddCommand(IAnsiConsole console) {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, PackageAddSettings settings, CancellationToken cancellationToken) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);
        var output = new TerminalOutput(_console);

        var root = Path.GetFullPath(settings.ProjectPath ?? Environment.CurrentDirectory);

        try {
            var manifest = ProjectManifest.Load(root);
            var (name, spec) = PackageSpec.Parse(settings.Package);

            if (settings.Source is { } source) {
                if (spec.Git is not null) {
                    throw new PackageException("--source can't be used with a git URL");
                }

                spec = spec with { Source = source };
            }

            var resolver = new Resolver(root, TypesInstall.Root, manifest.Sources, manifest.RobloxCSVersion, Lockfile.Load(root));

            var added = await output.RunAsync("resolve", $"Resolving [white]{Markup.Escape(name)}[/]", _ => resolver.ResolveOneAsync(name, spec, cancellationToken));

            if (spec.Git is null && spec.Version is null) {
                spec = spec with { Version = $"^{added.Version}" };
            }

            ManifestEditor.SetDependency(root, added.Name, spec);

            var dependencies = ProjectManifest.Load(root).Dependencies;
            var packages = await output.RunAsync("resolve", "Updating dependencies", _ => resolver.ResolveAllAsync(dependencies, cancellationToken));

            var pruned = PackageInstaller.Apply(root, manifest.RobloxCSVersion, packages);

            output.Write("added", "green", $"{Markup.Escape(added.Name)} [white]{Markup.Escape(added.Version)}[/] [grey]({added.Commit[..12]})[/]");

            var transitive = packages.Count - dependencies.Count;
            if (transitive > 0) {
                output.Write("detail", "grey", $"{transitive} transitive {(transitive == 1 ? "dependency" : "dependencies")}");
            }

            if (pruned > 0) {
                output.Write("removed", "grey", $"{pruned} unused {(pruned == 1 ? "package" : "packages")}");
            }

            return 0;
        } catch (Exception ex) when (ex is ManifestException or PackageSpecException or PackageException) {
            output.Write("error", "red", Markup.Escape(ex.Message));

            return -1;
        }
    }
}