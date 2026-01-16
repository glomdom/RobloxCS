using RobloxCS.Transpiler.Walkers;

namespace RobloxCS.Transpiler.Passes;

public sealed class CollectionsLoweringPass : IPass {
    public string Name => "Collections Lowering";
    public List<string> Diagnostics { get; } = [];

    public void Run(TranspilationContext ctx) {
        var walker = new CollectionsLoweringWalker(ctx.Registry);
        walker.Visit(ctx.RootBlock);
    }
}