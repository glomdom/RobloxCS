using RobloxCS.Transpiler.Walkers;

namespace RobloxCS.Transpiler.Passes;

/// <summary>
/// Links parents up the tree, making the AST a graph. So an ASG? rofl
/// </summary>
public sealed class LinkerPass : IPass {
    public void Run(TranspilationContext ctx) {
        var linker = new LinkerWalker(ctx);
        linker.LinkParents(ctx.RootBlock);
    }
}