using RobloxCS.Transpiler.Walkers;

namespace RobloxCS.Transpiler.Passes;

public sealed class ServiceLoweringPass : IPass {
    public string Name => "Service Lowering";

    private readonly ServiceLoweringWalker _walker = new();

    public void Run(TranspilationContext ctx) {
        _walker.Visit(ctx.RootBlock);
    }

    public void PostRun(TranspilationContext ctx) {
        var stmts = _walker.GetHeaderStatements();

        ctx.RootBlock.Statements.InsertRange(0, stmts);
    }
}