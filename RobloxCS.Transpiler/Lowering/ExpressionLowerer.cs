using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;

namespace RobloxCS.Transpiler.Lowering;

internal static class ExpressionLowerer {
    public static Expression LowerExpr(ExpressionSyntax syntax) => syntax switch {
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.StringLiteralExpression) => StringExpression.FromString(lit.Token.ValueText),
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.NumericLiteralExpression) => new NumberExpression { Value = Convert.ToDouble(lit.Token.Value) },
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.TrueLiteralExpression) => new BooleanExpression { Value = true },
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.FalseLiteralExpression) => new BooleanExpression { Value = false },

        _ => throw new ArgumentOutOfRangeException(nameof(syntax), $"Unsupported expression: {syntax.Kind()}"),
    };
}