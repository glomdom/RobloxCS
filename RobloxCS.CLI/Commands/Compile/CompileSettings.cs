using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Compile;

public abstract class CompileSettings : CommandSettings {
    [CommandOption("-v|--verbose")]
    [Description("Show verbose information, like type resolution and so on.")]
    public required bool Verbose { get; set; }
}