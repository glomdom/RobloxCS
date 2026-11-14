using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands;

public class CommonSettings : CommandSettings {
    [CommandOption("-v|--verbose")]
    [Description("Show compiler trace details alongside command progress")]
    public bool Verbosity { get; init; }
}