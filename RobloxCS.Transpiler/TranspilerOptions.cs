using RobloxCS.Lowering;
using RobloxCS.Modules;

namespace RobloxCS.Transpiler;

/// <param name="ScriptType">What kind of script the output is for.</param>
/// <param name="PrintHir">Dump the HIR to the console before lowering.</param>
public sealed record TranspilerOptions(ScriptType ScriptType, bool PrintHir = false) {
    public ModuleGraph Modules { get; init; } = ModuleGraph.Empty;
}