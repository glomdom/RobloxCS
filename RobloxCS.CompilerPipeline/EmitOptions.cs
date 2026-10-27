using RobloxCS.Modules;

namespace RobloxCS.CompilerPipeline;

/// <summary>
/// Options that apply to every file in a compile, as opposed to per-file ones like the script type.
/// </summary>
/// <param name="PrintHir">Dump each file's HIR to the console before lowering.</param>
public sealed record EmitOptions(bool PrintHir = false, bool SkipDiagnostics = false) {
    public ModuleGraph Modules { get; init; } = ModuleGraph.Empty;
}
