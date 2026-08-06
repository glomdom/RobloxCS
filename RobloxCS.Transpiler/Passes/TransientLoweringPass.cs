using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Passes;

/// <summary>
/// This pass lowers all <see cref="TransientStatement"/>s to their respective <see cref="Statement"/>s.
/// </summary>
public sealed class TransientLoweringPass : IPass {
    public string Name => "Transient Lowering";
    public List<string> Diagnostics { get; } = [];

    public void Run(TranspilationContext ctx) {
        // var walker = new TransientLoweringWalker();
        // ctx.RootBlock = (Block)walker.Visit(ctx.RootBlock);
    }
}