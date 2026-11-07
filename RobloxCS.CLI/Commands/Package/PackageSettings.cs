using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Package;

public class PackageSettings : CommonSettings {
    [CommandOption("--project")]
    [Description("Path to the project folder (defaults to the current directory)")]
    public string? ProjectPath { get; init; }
}