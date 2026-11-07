using JetBrains.Annotations;
using RobloxCS.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Add;

[UsedImplicitly]
public sealed class PackageAddCommand : AsyncCommand<PackageAddSettings> {
    private readonly IAnsiConsole _console;

    public PackageAddCommand(IAnsiConsole console) {
        _console = console;
    }
    
    protected override async Task<int> ExecuteAsync(CommandContext context, PackageAddSettings settings, CancellationToken cancellationToken) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);
        
        _console.MarkupLine("[gray]i wonder what this is [/]");
        
        return 0;
    }
}