using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands;

public class CommonSettings : CommandSettings {
    [CommandOption("-v|--verbose")]
    [Description("Verbose output for commands")]
    public bool Verbosity { get; set; }
}