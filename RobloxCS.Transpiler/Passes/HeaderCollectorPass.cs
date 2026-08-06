namespace RobloxCS.Transpiler.Passes;

public sealed class HeaderCollectorPass : IPass {
    public string Name => "Header Collector";
    public List<string> Diagnostics { get; } = [];

    public void Run(TranspilationContext ctx) {
        // var walker = new HeaderCollectorWalker(ctx);
        // walker.Visit(ctx.Root);
    }
}