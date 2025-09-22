using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.Compiler;
using RobloxCS.Transpiler.Builders;
using Serilog;

namespace RobloxCS.Transpiler;

public sealed class CSharpTranspiler : CSharpSyntaxWalker {
    public TranspilationContext Ctx { get; }

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Ctx = new TranspilationContext(options, compiler);
    }

    public Chunk Transpile() {
        Log.Information("Starting to transpile");
        var watch = Stopwatch.StartNew();

        Visit(Ctx.Root);

        watch.Stop();
        Log.Information("Finished transpiling in {Elapsed}ms", watch.ElapsedMilliseconds);

        return Ctx.ToChunk();
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        var classStatements = ClassBuilder.Build(node, Ctx);

        foreach (var stmt in classStatements) {
            Ctx.Add(stmt);
        }
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        // TODO: idk bro do we do this or not

        Log.Warning("Skipping emission for field group with {Count} variable(s).", node.Declaration.Variables.Count);
    }
}