using JetBrains.Annotations;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Remove;

[UsedImplicitly]
public sealed class PackageRemoveCommand : AsyncCommand<PackageRemoveSettings> {
    protected override async Task<int> ExecuteAsync(CommandContext context, PackageRemoveSettings settings, CancellationToken cancellationToken) {
        return 0;
    }
}