using RobloxCS.Transpiler.Walkers;

namespace RobloxCS.Transpiler.Passes;

/// <summary>
/// Links parents up the tree, making the AST a graph. So an ASG? rofl
/// </summary>
public sealed class LinkerPass : IPass {
    public string Name => "Linker";
    public List<string> Diagnostics { get; } = [];

    public void Run(TranspilationContext ctx) {
        var linker = new LinkerWalker(ctx);
        linker.LinkParents(ctx.RootBlock);
    }
}