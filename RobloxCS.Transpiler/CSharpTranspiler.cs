using RobloxCS.AST;
using RobloxCS.Compiler;
using RobloxCS.Transpiler.Passes;
using Serilog;

namespace RobloxCS.Transpiler;

public sealed class CSharpTranspiler {
    public TranspilationContext Ctx { get; }
    public PassManager PassManager { get; }

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Ctx = new TranspilationContext(options, compiler);
        PassManager = new PassManager();

        PassManager.Register(new HirPrinterPass());

        // TODO: FIX THIS GARBAGE..............
        // Ctx.RootBlock.AddStatement(StatementHelpers.UntypedLocalAssignment("List",
        //     ExpressionHelpers.SimpleFunctionCall("require",
        //         SymbolExpression.FromString("game:GetService(\"ReplicatedStorage\"):WaitForChild(\"robloxcs\"):WaitForChild(\"RuntimeLib\"):WaitForChild(\"List\")"))));
    }

    public Chunk Transpile() {
        var module = PassManager.Start(Ctx);
        if (module is null) {
            Log.Error("Failed to transpile");

            Environment.Exit(-1);
        }

        // return Ctx.ToChunk();

        return new Chunk {
            Block = new Block {
                Statements = [],
            }
        };
    }
}