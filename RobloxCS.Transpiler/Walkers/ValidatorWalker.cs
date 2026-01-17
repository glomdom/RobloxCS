using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;

namespace RobloxCS.Transpiler.Walkers;

public sealed class ValidatorWalker : CSharpSyntaxWalker {
    public bool FoundEntryPoint => EntryPointNames.Count != 0;
    public bool IsAmbiguousEntryPoint => EntryPointNames.Count != 1;
    public List<string> EntryPointNames { get; } = [];

    private readonly TranspilationContext _ctx;

    public ValidatorWalker(TranspilationContext ctx) {
        _ctx = ctx;
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node) {
        var methodSymbol = _ctx.Semantics.GetDeclaredSymbol(node);
        if (methodSymbol is null) {
            throw new Exception("Failed to get declaring symbol of method declaration.");
        }

        var attributes = methodSymbol.GetAttributes();
        Log.Verbose("Found attributes {AttributeList}", attributes);

        var isEntryPoint = attributes.Any(attr =>
            attr.AttributeClass?.Name == "EntryPointAttribute" &&
            attr.AttributeClass?.ContainingNamespace?.ToDisplayString() == "RobloxCS.Types.Attributes"
        );

        if (isEntryPoint) {
            EntryPointNames.Add(methodSymbol.Name);
        }

        base.VisitMethodDeclaration(node);
    }
}