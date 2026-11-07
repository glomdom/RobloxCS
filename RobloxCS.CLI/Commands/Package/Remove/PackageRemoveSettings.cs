using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Remove;

public sealed class PackageRemoveSettings : PackageSettings {
    [CommandArgument(0, "<package>")]
    [Description("Name of the dependency to remove, e.g. [white]owner/name[/]")]
    public required string Package { get; init; }
}