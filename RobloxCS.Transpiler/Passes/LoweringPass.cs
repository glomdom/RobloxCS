using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Transient;
using RobloxCS.Transpiler.Walkers;

namespace RobloxCS.Transpiler.Passes;

/// <summary>
/// This pass lowers all <see cref="TransientStatement"/>s to their respective <see cref="Statement"/>s.
/// </summary>
public sealed class LoweringPass : IPass {
    public void Run(TranspilationContext ctx) {
        var walker = new LoweringWalker();
        ctx.RootBlock = (Block)walker.Visit(ctx.RootBlock);
    }
}