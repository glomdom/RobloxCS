using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.Transpiler.Builders;

public static class VarBuilder {
    public static Var BuildFromExpressionSyntax(ExpressionSyntax expr, TranspilationContext ctx) {
        return expr switch {
            IdentifierNameSyntax nameSyntax => HandleIdentifierNameSyntax(nameSyntax, ctx),
            MemberAccessExpressionSyntax memberAccessSyntax => HandleMemberExpressionSyntax(memberAccessSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported expression in assignment: {expr.Kind()}"),
        };
    }

    private static Var HandleMemberExpressionSyntax(MemberAccessExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax.Kind() switch {
            SyntaxKind.SimpleMemberAccessExpression => HandleSimpleMemberAccessExpression(syntax, ctx),

            _ => throw new NotSupportedException($"Member access of type {syntax.Kind()} is not supported."),
        };
    }

    private static Var HandleSimpleMemberAccessExpression(MemberAccessExpressionSyntax syntax, TranspilationContext ctx) {
        return new VarExpression {
            Prefix = new ExpressionPrefix { Expression = ExpressionBuilder.BuildFromSyntax(syntax.Expression, ctx) },
            Suffixes = [
                new Dot {
                    Name = SymbolExpression.FromString(syntax.Name.Identifier.ValueText),
                },
            ],
        };
    }

    private static Var HandleIdentifierNameSyntax(IdentifierNameSyntax nameSyntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetSymbolInfo(nameSyntax).Symbol;
        if (symbol is null) throw new Exception($"Semantics failed to get symbol info for {nameSyntax.Identifier.ValueText}");

        return symbol switch {
            IFieldSymbol fieldSymbol => VarName.FromString($"self.{fieldSymbol.Name}"),
            ILocalSymbol localSymbol => VarName.FromString($"{localSymbol.Name}"),

            _ => throw new NotSupportedException($"Symbol of type {symbol.GetType().Name} is not supported."),
        };
    }
}