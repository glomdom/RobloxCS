using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;

namespace RobloxCS.Transpiler.Builders;

public class VarBuilder {
    public static Var BuildFromExpressionSyntax(ExpressionSyntax expr, TranspilationContext ctx) {
        return expr switch {
            IdentifierNameSyntax nameSyntax => HandleIdentifierNameSyntax(nameSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported expression in assignment: {expr.Kind()}"),
        };
    }

    private static Var HandleIdentifierNameSyntax(IdentifierNameSyntax nameSyntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetSymbolInfo(nameSyntax).Symbol;
        if (symbol is null) throw new Exception($"Semantics failed to get symbol info for {nameSyntax.Identifier.ValueText}");

        return symbol switch {
            IFieldSymbol fieldSymbol => VarName.FromString($"self.{fieldSymbol.Name}"),

            _ => throw new NotSupportedException($"Symbol of tyoe {symbol.GetType().Name} is not supported."),
        };
    }
}