using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.Compiler;
using RobloxCS.Transpiler.Helpers;
using RobloxCS.Transpiler.Passes;

namespace RobloxCS.Transpiler;

public sealed class CSharpTranspiler {
    public TranspilationContext Ctx { get; }
    public PassManager PassManager { get; }

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Ctx = new TranspilationContext(options, compiler);
        PassManager = new PassManager();

        PassManager.Register(new HeaderCollectorPass());
        PassManager.Register(new ConverterPass());
        PassManager.Register(new LinkerPass());
        PassManager.Register(new TransientLoweringPass());
        PassManager.Register(new ServiceLoweringPass());
        PassManager.Register(new CollectionsLoweringPass());

        // TODO: FIX THIS GARBAGE..............
        Ctx.RootBlock.AddStatement(StatementHelpers.UntypedLocalAssignment("List",
            ExpressionHelpers.SimpleFunctionCall("require",
                SymbolExpression.FromString("game:GetService(\"ReplicatedStorage\"):WaitForChild(\"robloxcs\"):WaitForChild(\"RuntimeLib\"):WaitForChild(\"List\")"))));
    }

    public Chunk Transpile() {
        PassManager.Run(Ctx);

        return Ctx.ToChunk();
    }
}