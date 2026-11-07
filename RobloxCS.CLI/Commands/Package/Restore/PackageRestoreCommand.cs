using JetBrains.Annotations;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Restore;

[UsedImplicitly]
public sealed class PackageRestoreCommand : AsyncCommand<PackageRestoreSettings> {
    protected override async Task<int> ExecuteAsync(CommandContext context, PackageRestoreSettings settings, CancellationToken cancellationToken) {
        return 0;
    }
}