using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.Transpiler.Semantics;

namespace RobloxCS.Transpiler.Walkers;

public sealed class HeaderCollectorWalker : CSharpSyntaxWalker {
    private readonly TranspilationContext _ctx;
    private readonly GlobalRegistry _registry;

    public HeaderCollectorWalker(TranspilationContext ctx) {
        _ctx = ctx;
        _registry = ctx.Registry;
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        var classSymbol = _ctx.Semantics.GetDeclaredSymbol(node);
        if (classSymbol is null) return;

        _registry.RegisterClass(classSymbol.Name);

        base.VisitClassDeclaration(node);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
        var propSymbol = _ctx.Semantics.GetDeclaredSymbol(node);
        if (propSymbol == null) return;

        var type = SyntaxUtilities.TypeInfoFromSymbol(propSymbol.Type, _ctx);

        _registry.RegisterMember(propSymbol.ContainingType.Name, propSymbol.Name, type);
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        foreach (var symbol in node.Declaration.Variables.Select(variable => _ctx.Semantics.GetDeclaredSymbol(variable))) {
            if (symbol is not IFieldSymbol fieldSymbol) continue;

            var type = SyntaxUtilities.TypeInfoFromSymbol(fieldSymbol.Type, _ctx);
            _registry.RegisterMember(fieldSymbol.ContainingType.Name, fieldSymbol.Name, type);
        }
    }
}