using System.ComponentModel;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Build;

public sealed class BuildSettings : CommonSettings {
    [CommandOption("--project")]
    [Description("Path to the Rojo project file")]
    public string ProjectJsonPath { get; init; } = "default.project.json";

    [CommandOption("-w|--watch")]
    [Description("Keep running and recompile files as they change")]
    public bool Watch { get; init; }

    [CommandOption("--no-diagnostics")]
    [Description("Don't run Roslyn diagnostics")]
    public bool SkipDiagnostics { get; init; }

    [CommandOption("--print-hir")]
    [Description("Dump the HIR of each file before lowering")]
    public bool PrintHir { get; init; }
}