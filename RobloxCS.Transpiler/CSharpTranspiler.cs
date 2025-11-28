using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.Compiler;
using RobloxCS.Transpiler.Builders;
using RobloxCS.Transpiler.Passes;
using Serilog;

namespace RobloxCS.Transpiler;

public sealed class CSharpTranspiler {
    public TranspilationContext Ctx { get; }
    public PassManager PassManager { get; }

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Ctx = new TranspilationContext(options, compiler);
        PassManager = new PassManager();
        
        PassManager.Register(new ConverterPass());
    }

    public Chunk Transpile() {
        PassManager.Run(Ctx);

        return Ctx.ToChunk();
    }
}