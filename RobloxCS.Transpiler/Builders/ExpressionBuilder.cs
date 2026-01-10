using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Transient;
using RobloxCS.Transpiler.Helpers;
using RobloxCS.Transpiler.Macros;
using RobloxCS.Types;
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
            MemberAccessExpressionSyntax memberAccessExpressionSyntax => HandleMemberAccessExpressionSyntax(memberAccessExpressionSyntax, ctx),
            SimpleLambdaExpressionSyntax simpleLambdaExpressionSyntax => HandleSimpleLambdaExpressionSyntax(simpleLambdaExpressionSyntax, ctx),
            ParenthesizedExpressionSyntax parenthesizedExpressionSyntax => HandleParenthesizedExpressionSyntax(parenthesizedExpressionSyntax, ctx),
            ObjectCreationExpressionSyntax objectCreationExpressionSyntax => HandleObjectCreationExpressionSyntax(objectCreationExpressionSyntax, ctx),
            ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpressionSyntax => HandleParenthesizedLambdaExpressionSyntax(parenthesizedLambdaExpressionSyntax, ctx),

            _ => throw new NotSupportedException($"Expression {syntax.Kind()} is not supported. {syntax}"),
        };
    }

    // TODO: Fixme
    private static Expression HandleSimpleLambdaExpressionSyntax(SimpleLambdaExpressionSyntax syntax, TranspilationContext ctx) {
        return ExpressionHelpers.SimpleAnonymousFunction([StatementHelpers.EmptyReturnStatement()]);
    }

    // TODO: Fixme
    private static Expression HandleParenthesizedLambdaExpressionSyntax(ParenthesizedLambdaExpressionSyntax syntax, TranspilationContext ctx) {
        return ExpressionHelpers.SimpleAnonymousFunction([StatementHelpers.EmptyReturnStatement()]);
    }

    private static VarExpression HandleMemberAccessExpressionSyntax(MemberAccessExpressionSyntax syntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetSymbol(syntax);

        return symbol switch {
            IFieldSymbol fieldSymbol => new VarExpression {
                Prefix = new ExpressionPrefix {
                    Expression = BuildFromSyntax(syntax.Expression, ctx)
                },
                Suffixes = [
                    new Dot {
                        Name = SymbolExpression.FromString(fieldSymbol.Name),
                    },
                ],
            },

            // TODO: Fixme
            IPropertySymbol propSymbol => new VarExpression {
                Prefix = new ExpressionPrefix {
                    Expression = BuildFromSyntax(syntax.Expression, ctx)
                },
                Suffixes = [
                    new Dot {
                        Name = SymbolExpression.FromString(propSymbol.Name),
                    },
                ],
            },

            _ => throw new NotSupportedException($"Unsupported member access: {symbol.GetType().Name}")
        };
    }

    private static Expression HandleObjectCreationExpressionSyntax(ObjectCreationExpressionSyntax syntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetSymbolInfo(syntax);
        if (symbol.Symbol is not IMethodSymbol ctorSymbol) {
            throw new Exception("Constructor symbol missing from object creation expression.");
        }

        List<Expression> exprs = [];
        if (syntax.ArgumentList is not null) {
            var args = syntax.ArgumentList.Arguments;
            foreach (var arg in args) { }

            exprs = syntax.ArgumentList.Arguments.Select(a => BuildFromSyntax(a.Expression, ctx)).ToList();
        }

        var classSymbol = ctorSymbol.ContainingType;
        var classNs = classSymbol.ContainingNamespace;
        var isRobloxType = classNs is not null && classNs.ToDisplayString() == "RobloxCS.Types";
        if (!isRobloxType) return ExpressionHelpers.DirectFunctionCall(syntax.Type.ToString(), "new", exprs);

        var nativeAttribute = SyntaxUtilities.ExtractAttributeFromAttributes<RobloxNativeAttribute>(classSymbol.GetAttributes());
        if (nativeAttribute.NativeType == RobloxNativeType.Instance) {
            Log.Verbose("Rewriting instance creation of {InstanceName} to use Instance.new", nativeAttribute.RobloxName);

            List<Expression> prependedExprs = [StringExpression.FromString(nativeAttribute.RobloxName), ..exprs];

            return ExpressionHelpers.DirectFunctionCall("Instance", "new", prependedExprs);
        }

        return ExpressionHelpers.DirectFunctionCall(syntax.Type.ToString(), "new", exprs);
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

        var key = MacroManager.GetMacroKey(methodSymbol);
        var got = MacroManager.TryGetMethodMacro(key, out var handler);

        if (got) {
            Log.Debug("Querying {SymbolName} inside macro registry {Got}", key, got);

            return handler!(syntax, ctx);
        } else {
            Log.Debug("Querying {SymbolName} inside macro registry {Got}", key, got);
        }

        var args = syntax.ArgumentList.Arguments.Select(a => BuildFromSyntax(a.Expression, ctx)).ToList();
        var functionArgs = ExpressionHelpers.FunctionArgsFromExpressions(args);

        var methodContainer = methodSymbol.ContainingSymbol;
        if (methodContainer is not null) {
            var ns = methodContainer.ContainingNamespace;
            var isRoblox = ns.ToDisplayString() == "RobloxCS.Types";

            if (isRoblox) {
                var nativeAttribute = SyntaxUtilities.ExtractAttributeFromAttributes<RobloxNativeAttribute>(methodContainer.GetAttributes());
                if (nativeAttribute.NativeType == RobloxNativeType.Service) {
                    var call = new FunctionCallExpression {
                        Prefix = new NamePrefix { Name = methodSymbol.Name },
                        Suffixes = [
                            new AnonymousCall {
                                Arguments = functionArgs,
                            },
                        ],
                    };

                    return new TransientServiceUsageExpression {
                        ServiceName = nativeAttribute.RobloxName,
                        AccessExpression = call,
                    };
                }
            }
        }

        if (methodSymbol.IsStatic) throw new Exception("Static methods are not yet supported.");

        if (SyntaxUtilities.IsInvokedOnExternalObject(syntax, ctx.Semantics, out var receiver)) {
            var obj = BuildFromSyntax((ExpressionSyntax)receiver.Syntax, ctx);

            var call = new FunctionCallExpression {
                Prefix = new ExpressionPrefix { Expression = obj },
                Suffixes = [
                    new MethodCall {
                        Name = methodSymbol.Name,
                        Args = functionArgs,
                    },
                ],
            };

            return call;
        }

        var selfCall = ExpressionHelpers.SimpleMethodCall("self", methodSymbol.Name, functionArgs);

        return selfCall;
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

        // TODO: Support right being string as well as left and right being string

        if (leftResult is StringExpression leftString) {
            return new InterpolatedStringExpression {
                Segments = [new InterpolatedStringSegment { Literal = leftString.Value, Expression = rightResult }],
                LastString = string.Empty,
            };
        }

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