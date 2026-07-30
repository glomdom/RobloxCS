using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands;

public class CommonSettings : CommandSettings {
    [CommandOption("-v|--verbose")]
    [Description("Verbose output for commands")]
    public bool Verbosity { get; init; }

    [CommandOption("--types-file")]
    [Description("File path to the assembly containing type definitions")]
    public string TypesFilePath { get; init; } = "./RobloxCS.Types.dll";
    
    [CommandOption("--no-diagnostics")]
    [Description("Will diagnostics be enabled for the compiler")]
    public bool SkipDiagnostics { get; init; }
}