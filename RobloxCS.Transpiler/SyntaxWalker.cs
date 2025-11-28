using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.Transpiler.Builders;

namespace RobloxCS.Transpiler;

public sealed class SyntaxWalker : CSharpSyntaxWalker {
    public TranspilationContext Ctx { get; }

    public SyntaxWalker(TranspilationContext ctx) {
        Ctx = ctx;
    }
    
    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        var classStatements = ClassBuilder.Build(node, Ctx);

        foreach (var stmt in classStatements) {
            Ctx.Add(stmt);
        }

        base.VisitClassDeclaration(node);
    }
}