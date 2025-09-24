using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;

namespace RobloxCS.Transpiler.Builders;

public class ExpressionBuilder {
    public static Expression BuildFromSyntax(ExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax switch {
            IdentifierNameSyntax nameSyntax => HandleIdentifierNameSyntax(nameSyntax, ctx),
            LiteralExpressionSyntax exprSyntax => HandleLiteralExpressionSyntax(exprSyntax, ctx),
            BinaryExpressionSyntax binExprSyntax => HandleBinaryExpressionSyntax(binExprSyntax, ctx),
            PrefixUnaryExpressionSyntax prefixUnaryExprSyntax => HandleUnaryExpressionSyntax(prefixUnaryExprSyntax, ctx),

            _ => throw new NotSupportedException($"Expression {syntax.Kind()} is not supported. {syntax}"),
        };
    }

    private static Expression HandleUnaryExpressionSyntax(PrefixUnaryExpressionSyntax syntax, TranspilationContext ctx) {
        var tOp = SyntaxUtilities.SyntaxTokenToUnOp(syntax.OperatorToken);
        var tOperand = BuildFromSyntax(syntax.Operand, ctx);

        return new UnaryOperatorExpression { UnOp = tOp, Expression = tOperand };
    }

    private static Expression HandleBinaryExpressionSyntax(BinaryExpressionSyntax syntax, TranspilationContext ctx) {
        var left = syntax.Left;
        var right = syntax.Right;

        var tLeft = BuildFromSyntax(left, ctx);
        var tRight = BuildFromSyntax(right, ctx);
        var tOp = SyntaxUtilities.SyntaxTokenToBinOp(syntax.OperatorToken);

        return new BinaryOperatorExpression {
            Left = tLeft,
            Right = tRight,
            Op = tOp,
        };
    }

    private static Expression HandleLiteralExpressionSyntax(LiteralExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax.Kind() switch {
            SyntaxKind.NumericLiteralExpression => HandleNumericLiteralExpression(syntax, ctx),

            _ => throw new NotSupportedException($"LiteralExpressionSyntax {syntax.Kind()} is not supported."),
        };
    }

    private static NumberExpression HandleNumericLiteralExpression(LiteralExpressionSyntax syntax, TranspilationContext ctx) {
        var value = syntax.Token.Value!;

        return NumberExpression.From(Convert.ToDouble(value));
    }

    private static Expression HandleIdentifierNameSyntax(IdentifierNameSyntax syntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetSymbolInfo(syntax).Symbol;
        if (symbol is null) throw new Exception($"Semantics failed to get symbol info for {syntax.Identifier.ValueText}.");

        return symbol switch {
            IParameterSymbol parameterSymbol => SymbolExpression.FromString(parameterSymbol.Name),
            ILocalSymbol localSymbol => SymbolExpression.FromString(localSymbol.Name),
            IFieldSymbol fieldSymbol => SymbolExpression.FromString($"self.{fieldSymbol.Name}"),

            _ => throw new NotSupportedException($"IdentifierNameSyntax {symbol.Kind} is not supported."),
        };
    }
}