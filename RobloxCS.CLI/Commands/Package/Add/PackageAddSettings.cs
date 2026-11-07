using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package.Add;

public sealed class PackageAddSettings : PackageSettings {
    [CommandArgument(0, "<package>")]
    [Description("Package to add, e.g. [white]todo list example packages here[/]")]
    public required string Package { get; init; }
}