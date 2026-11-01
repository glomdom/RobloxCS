using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Init;

public sealed class InitSettings : CommonSettings {
    [CommandArgument(0, "[name]")]
    [Description("Name of the new project. Creates a folder with this name; omit to initialize the current folder")]
    public string? Name { get; init; }

    [CommandOption("-f|--force")]
    [Description("Overwrite files that already exist")]
    public bool Force { get; init; }
}