using RobloxCS.AST.Statements;
using RobloxCS.AST.Transient;
using Serilog;

namespace RobloxCS.Transpiler.Passes;

/// <summary>
/// This pass lowers all <see cref="TransientStatement"/>s to their respective <see cref="Statement"/>s.
/// </summary>
public sealed class LoweringPass : IPass {
    public void Run(TranspilationContext ctx) {
        Log.Information("Hi, I'm a stub for the lowering pass!");
    }
}