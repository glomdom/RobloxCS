using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Add;

public sealed class PackageAddSettings : PackageSettings {
    [CommandArgument(0, "<package>")]
    [Description("Package to add: [white]owner/name[/], [white]owner/name@^1.2[/], or a git URL like [white]https://github.com/owner/repo@v1.0.0[/]")]
    public required string Package { get; init; }

    [CommandOption("-s|--source")]
    [Description("Package source to look the name up in (default: the central source)")]
    public string? Source { get; init; }
}