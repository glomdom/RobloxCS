using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.Transpiler.Builders;

public class ExpressionBuilder {
    public static ExpressionBuilderResult BuildFromSyntax(ExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax switch {
            IdentifierNameSyntax nameSyntax => HandleIdentifierNameSyntax(nameSyntax, ctx),
            LiteralExpressionSyntax exprSyntax => HandleLiteralExpressionSyntax(exprSyntax, ctx),
            BinaryExpressionSyntax binExprSyntax => HandleBinaryExpressionSyntax(binExprSyntax, ctx),
            PrefixUnaryExpressionSyntax prefixUnaryExprSyntax => HandleUnaryExpressionSyntax(prefixUnaryExprSyntax, ctx),
            InvocationExpressionSyntax invocationExpressionSyntax => HandleInvocationExpressionSyntax(invocationExpressionSyntax, ctx),
            ConditionalExpressionSyntax conditionalExpressionSyntax => HandleConditionalExpressionSyntax(conditionalExpressionSyntax, ctx),

            _ => throw new NotSupportedException($"Expression {syntax.Kind()} is not supported. {syntax}"),
        };
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

        return ExpressionBuilderResult.FromSingle(expr);
    }

    private static ExpressionBuilderResult HandleBinaryExpressionSyntax(BinaryExpressionSyntax syntax, TranspilationContext ctx) {
        var left = syntax.Left;
        var right = syntax.Right;

        var tLeft = BuildFromSyntax(left, ctx);
        var tRight = BuildFromSyntax(right, ctx);
        var tOp = SyntaxUtilities.SyntaxTokenToBinOp(syntax.OperatorToken);
        var expr = new BinaryOperatorExpression {
            Left = tLeft.Expression,
            Right = tRight.Expression,
            Op = tOp,
        };

        return ExpressionBuilderResult.FromSingle(expr);
    }

    private static ExpressionBuilderResult HandleLiteralExpressionSyntax(LiteralExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax.Kind() switch {
            SyntaxKind.NumericLiteralExpression => HandleNumericLiteralExpression(syntax, ctx),

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
            IFieldSymbol fieldSymbol => ExpressionBuilderResult.FromSingle(SymbolExpression.FromString($"self.{fieldSymbol.Name}")),

            _ => throw new NotSupportedException($"IdentifierNameSyntax {symbol.Kind} is not supported."),
        };
    }
}