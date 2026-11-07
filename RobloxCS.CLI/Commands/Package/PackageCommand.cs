using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package;

public sealed class PackageCommand : Command<PackageSettings> {
    protected override int Execute(CommandContext context, PackageSettings settings, CancellationToken cancellationToken) {
        return -1;
    }
}