using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;

namespace RobloxCS.Transpiler.Builders;

public class ExpressionBuilder {
    public static Expression BuildFromExpressionStatement(ExpressionStatementSyntax exprStmtSyntax) {
        throw new NotImplementedException();
    }

    public static Expression BuildFromSyntax(ExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax switch {
            IdentifierNameSyntax nameSyntax => HandleIdentifierNameSyntax(nameSyntax, ctx),

            _ => throw new NotSupportedException($"Expression {syntax.Kind()} is not supported."),
        };
    }

    private static Expression HandleIdentifierNameSyntax(IdentifierNameSyntax nameSyntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetSymbolInfo(nameSyntax).Symbol;
        if (symbol is null) throw new Exception($"Semantics failed to get symbol info for {nameSyntax.Identifier.ValueText}");

        return symbol switch {
            IParameterSymbol parameterSymbol => SymbolExpression.FromString(parameterSymbol.Name),
            ILocalSymbol localSymbol => SymbolExpression.FromString(localSymbol.Name),

            _ => throw new NotSupportedException($"Symbol of type {symbol.GetType().Name} is not supported."),
        };
    }
}