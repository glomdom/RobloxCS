using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;
using RobloxCS.Transpiler.Builders;

namespace RobloxCS.Transpiler.Lowering;

internal static class ExpressionLowerer {
    public static Expression LowerExpr(ExpressionSyntax syntax, TranspilationContext ctx) => syntax switch {
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.StringLiteralExpression) => StringExpression.FromString(lit.Token.ValueText),
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.NumericLiteralExpression) => new NumberExpression { Value = Convert.ToDouble(lit.Token.Value) },
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.TrueLiteralExpression) => new BooleanExpression { Value = true },
        LiteralExpressionSyntax lit when lit.IsKind(SyntaxKind.FalseLiteralExpression) => new BooleanExpression { Value = false },
        CollectionExpressionSyntax col => new TableConstructorExpression {
            Fields = col.Elements.Select(e => TypeFieldBuilder.BuildFromSyntax(e switch {
                ExpressionElementSyntax exprElem => exprElem.Expression,

                _ => throw new NotSupportedException($"Unsupported element type: {e.Kind()}."),
            }, ctx)).ToList(),
        },

        _ => throw new ArgumentOutOfRangeException(nameof(syntax), $"Unsupported expression: {syntax.Kind()}"),
    };
}