using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;
using RobloxCS.Transpiler.Helpers;
using RobloxCS.Transpiler.Macros;
using Serilog;

namespace RobloxCS.Transpiler.Builders;

public static class ExpressionBuilder {
    public static Expression BuildFromSyntax(ExpressionSyntax syntax, TranspilationContext ctx) {
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

    private static ParenthesisExpression HandleParenthesizedExpressionSyntax(ParenthesizedExpressionSyntax syntax, TranspilationContext ctx) {
        var inner = BuildFromSyntax(syntax.Expression, ctx);

        return ExpressionHelpers.ParenthesisFromInner(inner);
    }

    private static Expression HandleConditionalExpressionSyntax(ConditionalExpressionSyntax syntax, TranspilationContext ctx) {
        var cond = BuildFromSyntax(syntax.Condition, ctx);
        var whenFalse = BuildFromSyntax(syntax.WhenFalse, ctx);
        var whenTrue = BuildFromSyntax(syntax.WhenTrue, ctx);

        var ifExpr = IfExpression.From(cond, whenTrue, whenFalse);

        return ifExpr;
    }

    private static Expression HandleInvocationExpressionSyntax(InvocationExpressionSyntax syntax, TranspilationContext ctx) {
        var info = ctx.Semantics.GetSymbolInfo(syntax);
        if (info.Symbol is not IMethodSymbol methodSymbol) throw new Exception("Invocation expression is not a method.");
        
        Log.Debug("Querying {SymbolName} inside macro registry", MacroManager.GetMacroKey(methodSymbol));

        var key = MacroManager.GetMacroKey(methodSymbol);
        if (MacroManager.TryGetMethodMacro(key, out var handler)) {
            return handler(syntax, ctx);
        }
        
        if (methodSymbol.IsStatic) throw new Exception("Static methods are not yet supported.");

        var methodName = $"self:{methodSymbol.Name}";
        var args = syntax.ArgumentList.Arguments.Select(ars => BuildFromSyntax(ars.Expression, ctx)).ToList();

        var call = ExpressionHelpers.SimpleFunctionCall(methodName, args);

        return call;
    }

    private static Expression HandlePostfixExpressionSyntax(PostfixUnaryExpressionSyntax syntax, TranspilationContext ctx) {
        var tOp = SyntaxUtilities.SyntaxTokenToUnOp(syntax.OperatorToken);
        var tOperand = BuildFromSyntax(syntax.Operand, ctx);
        var expr = new UnaryOperatorExpression { UnOp = tOp, Expression = tOperand };

        return expr;
    }

    private static Expression HandleUnaryExpressionSyntax(PrefixUnaryExpressionSyntax syntax, TranspilationContext ctx) {
        var tOp = SyntaxUtilities.SyntaxTokenToUnOp(syntax.OperatorToken);
        var tOperand = BuildFromSyntax(syntax.Operand, ctx);
        var expr = new UnaryOperatorExpression { UnOp = tOp, Expression = tOperand };
        var parenExpr = ParenthesisExpression.From(expr);

        return parenExpr;
    }

    private static Expression HandleBinaryExpressionSyntax(BinaryExpressionSyntax syntax, TranspilationContext ctx) {
        var left = syntax.Left;
        var right = syntax.Right;

        var leftResult = BuildFromSyntax(left, ctx);
        var rightResult = BuildFromSyntax(right, ctx);
        var op = SyntaxUtilities.SyntaxTokenToBinOp(syntax.OperatorToken);
        var expr = new BinaryOperatorExpression {
            Left = leftResult,
            Right = rightResult,
            Op = op,
        };

        return expr;
    }

    private static Expression HandleLiteralExpressionSyntax(LiteralExpressionSyntax syntax, TranspilationContext ctx) {
        return syntax.Kind() switch {
            SyntaxKind.NumericLiteralExpression => HandleNumericLiteralExpression(syntax, ctx),
            SyntaxKind.StringLiteralExpression => HandleStringLiteralExpression(syntax, ctx),
            SyntaxKind.FalseLiteralExpression => BooleanExpression.False(),
            SyntaxKind.TrueLiteralExpression => BooleanExpression.True(),

            _ => throw new NotSupportedException($"LiteralExpressionSyntax {syntax.Kind()} is not supported."),
        };
    }
    
    private static StringExpression HandleStringLiteralExpression(LiteralExpressionSyntax syntax, TranspilationContext ctx) {
        var value = (string)syntax.Token.Value!;

        return StringExpression.FromString(value);
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
            IFieldSymbol fieldSymbol => HandleIFieldSymbol(fieldSymbol),

            _ => throw new NotSupportedException($"IdentifierNameSyntax {symbol.Kind} is not supported."),
        };
    }

    private static Expression HandleIFieldSymbol(IFieldSymbol fieldSymbol) {
        var fieldName = fieldSymbol.IsStatic ? $"{fieldSymbol.ContainingSymbol.Name}.{fieldSymbol.Name}" : $"self.{fieldSymbol.Name}";

        return SymbolExpression.FromString(fieldName);
    }
}