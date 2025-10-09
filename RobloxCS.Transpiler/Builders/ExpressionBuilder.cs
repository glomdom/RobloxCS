using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.Transpiler.Builders;

public static class ExpressionBuilder {
    public static ExpressionBuilderResult BuildFromSyntax(ExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax switch {
            IdentifierNameSyntax nameSyntax => HandleIdentifierNameSyntax(nameSyntax, ctx),
            LiteralExpressionSyntax exprSyntax => HandleLiteralExpressionSyntax(exprSyntax, ctx),
            BinaryExpressionSyntax binExprSyntax => HandleBinaryExpressionSyntax(binExprSyntax, ctx),
            PrefixUnaryExpressionSyntax prefixUnaryExprSyntax => HandleUnaryExpressionSyntax(prefixUnaryExprSyntax, ctx),
            InvocationExpressionSyntax invocationExpressionSyntax => HandleInvocationExpressionSyntax(invocationExpressionSyntax, ctx),
            ConditionalExpressionSyntax conditionalExpressionSyntax => HandleConditionalExpressionSyntax(conditionalExpressionSyntax, ctx),
            ParenthesizedExpressionSyntax parenthesizedExpressionSyntax => HandleParenthesizedExpressionSyntax(parenthesizedExpressionSyntax, ctx),

            _ => throw new NotSupportedException($"Expression {syntax.Kind()} is not supported. {syntax}"),
        };
    }

    private static ExpressionBuilderResult HandleParenthesizedExpressionSyntax(ParenthesizedExpressionSyntax syntax, TranspilationContext ctx) {
        var exprResult = BuildFromSyntax(syntax.Expression, ctx);

        return ExpressionBuilderResult.FromSingle(exprResult.Expression);
    }

    private static ExpressionBuilderResult HandleConditionalExpressionSyntax(ConditionalExpressionSyntax syntax, TranspilationContext ctx) {
        var condResult = BuildFromSyntax(syntax.Condition, ctx);
        var whenFalseResult = BuildFromSyntax(syntax.WhenFalse, ctx);
        var whenTrueResult = BuildFromSyntax(syntax.WhenTrue, ctx);

        var ifExpr = IfExpression.From(condResult.Expression, whenTrueResult.Expression, whenFalseResult.Expression);
        var result = ExpressionBuilderResult.FromSingle(ifExpr);

        return result;
    }

    private static ExpressionBuilderResult HandleInvocationExpressionSyntax(InvocationExpressionSyntax syntax, TranspilationContext ctx) {
        var info = ctx.Semantics.GetSymbolInfo(syntax);
        if (info.Symbol is not IMethodSymbol methodSymbol) throw new Exception("Invocation expression is not a method.");

        if (methodSymbol.IsStatic) throw new Exception("Static methods are not yet supported.");

        var methodName = $"self:{methodSymbol.Name}";
        var argExpressions = syntax.ArgumentList.Arguments.Select(ars => BuildFromSyntax(ars.Expression, ctx).Expression).ToList();

        var call = new FunctionCallExpression { Prefix = NamePrefix.FromString(methodName), Suffixes = [AnonymousCall.FromArgs(FunctionArgs.FromExpressions(argExpressions))] };

        return ExpressionBuilderResult.FromSingle(call);
    }

    private static ExpressionBuilderResult HandlePostfixExpressionSyntax(PostfixUnaryExpressionSyntax syntax, TranspilationContext ctx) {
        var tOp = SyntaxUtilities.SyntaxTokenToUnOp(syntax.OperatorToken);
        var tOperand = BuildFromSyntax(syntax.Operand, ctx);
        var expr = new UnaryOperatorExpression { UnOp = tOp, Expression = tOperand.Expression };

        return ExpressionBuilderResult.FromSingle(expr);
    }

    private static ExpressionBuilderResult HandleUnaryExpressionSyntax(PrefixUnaryExpressionSyntax syntax, TranspilationContext ctx) {
        var tOp = SyntaxUtilities.SyntaxTokenToUnOp(syntax.OperatorToken);
        var tOperand = BuildFromSyntax(syntax.Operand, ctx);
        var expr = new UnaryOperatorExpression { UnOp = tOp, Expression = tOperand.Expression };
        var parenExpr = ParenthesisExpression.From(expr);

        return ExpressionBuilderResult.FromSingle(parenExpr);
    }

    private static ExpressionBuilderResult HandleBinaryExpressionSyntax(BinaryExpressionSyntax syntax, TranspilationContext ctx) {
        var left = syntax.Left;
        var right = syntax.Right;

        var leftResult = BuildFromSyntax(left, ctx);
        var rightResult = BuildFromSyntax(right, ctx);
        var op = SyntaxUtilities.SyntaxTokenToBinOp(syntax.OperatorToken);
        var expr = new BinaryOperatorExpression {
            Left = leftResult.Expression,
            Right = rightResult.Expression,
            Op = op,
        };

        return ExpressionBuilderResult.FromSingle(expr);
    }

    private static ExpressionBuilderResult HandleLiteralExpressionSyntax(LiteralExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax.Kind() switch {
            SyntaxKind.NumericLiteralExpression => HandleNumericLiteralExpression(syntax, ctx),
            SyntaxKind.FalseLiteralExpression => ExpressionBuilderResult.FromSingle(BooleanExpression.False()),
            SyntaxKind.TrueLiteralExpression => ExpressionBuilderResult.FromSingle(BooleanExpression.True()),

            _ => throw new NotSupportedException($"LiteralExpressionSyntax {syntax.Kind()} is not supported."),
        };
    }

    private static ExpressionBuilderResult HandleNumericLiteralExpression(LiteralExpressionSyntax syntax, TranspilationContext ctx) {
        var value = syntax.Token.Value!;

        return ExpressionBuilderResult.FromSingle(NumberExpression.From(Convert.ToDouble(value)));
    }

    private static ExpressionBuilderResult HandleIdentifierNameSyntax(IdentifierNameSyntax syntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetSymbolInfo(syntax).Symbol;
        if (symbol is null) throw new Exception($"Semantics failed to get symbol info for {syntax.Identifier.ValueText}.");

        return symbol switch {
            IParameterSymbol parameterSymbol => ExpressionBuilderResult.FromSingle(SymbolExpression.FromString(parameterSymbol.Name)),
            ILocalSymbol localSymbol => ExpressionBuilderResult.FromSingle(SymbolExpression.FromString(localSymbol.Name)),
            IFieldSymbol fieldSymbol => HandleIFieldSymbol(fieldSymbol),

            _ => throw new NotSupportedException($"IdentifierNameSyntax {symbol.Kind} is not supported."),
        };
    }

    private static ExpressionBuilderResult HandleIFieldSymbol(IFieldSymbol fieldSymbol) {
        var fieldName = fieldSymbol.IsStatic ? $"{fieldSymbol.ContainingSymbol.Name}.{fieldSymbol.Name}" : $"self.{fieldSymbol.Name}";

        return ExpressionBuilderResult.FromSingle(SymbolExpression.FromString(fieldName));
    }
}