using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Compile;

public abstract class CompileSettings : CommonSettings {
    [CommandOption("--types-file")]
    [Description("File path to the assembly containing type definitions")]
    public string TypesFilePath { get; init; } = "./RobloxCS.Types.dll";

    [CommandOption("--no-diagnostics")]
    [Description("Don't run Roslyn diagnostics")]
    public bool SkipDiagnostics { get; init; }

    [CommandOption("--print-hir")]
    [Description("Dump the HIR of each file before lowering")]
    public bool PrintHir { get; init; }
}