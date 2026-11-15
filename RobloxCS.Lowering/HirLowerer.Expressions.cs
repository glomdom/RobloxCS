using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR.Expressions;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerExpression(HirExpression expression) {
        switch (expression) {
            case HirCollectionLiteral collection: {
                return LowerCollection(collection);
            }

            case HirIndexAccess index: {
                return LowerCollectionIndex(index);
            }

            case HirDefault value: {
                return DefaultValue(value.Type);
            }

            case HirLiteral literal: {
                return LowerLiteral(literal);
            }

            case HirLocalRef local: {
                return Name(_names.NameFor(local.Symbol));
            }

            case HirParameterRef parameter: {
                return LowerParameterReference(parameter.Symbol);
            }

            case HirThis: {
                return Name("self");
            }

            case HirFieldAccess field: {
                return LowerFieldAccess(field);
            }

            case HirCall call: {
                if (IsLuaTuple(call.Method.ContainingType)) {
                    return LowerTupleCall(call);
                }

                if (ReturnsMultipleValues(call.Method)) {
                    return LowerCall(call) is { } multiple ? PackTuple([multiple]) : Nil();
                }

                var value = LowerCall(call) ?? (Expression)Nil();
                if (IsList(call.Method.ContainingType) && call.Method.Name is "Find" or "FindLast") {
                    return Cast(value, LowerType(call.Type, call.Location));
                }

                return value;
            }

            case HirObjectCreation creation: {
                return LowerObjectCreation(creation);
            }

            case HirBinary binary: {
                return LowerBinary(binary);
            }

            case HirIncrementDecrement increment: {
                Unsupported("++ and -- can only be used as statements yet.", increment.Location);

                return Nil();
            }

            case HirConversion conversion: {
                return LowerConversion(conversion);
            }

            case HirPropertyAccess access: {
                return LowerPropertyAccess(access);
            }

            case HirLambda lambda: {
                return LowerLambda(lambda);
            }

            case HirUnary unary: {
                if (IsSourceEnum(unary.Type) && unary.Op == UnaryOperatorKind.BitwiseNegation) {
                    return LowerEnumBits(unary.Type, "bnot", [LowerExpression(unary.Operand)], unary.Location);
                }

                if (TryLowerDataTypeOperator(unary, out var dataTypeUnary)) {
                    return dataTypeUnary;
                }

                if (unary.OperatorMethod is not null) {
                    Unsupported($"User-defined operator '{unary.OperatorMethod.ToDisplayString()}' is not supported yet.", unary.Location);

                    return Nil();
                }

                UnOp? op = unary.Op switch {
                    UnaryOperatorKind.Minus => UnOp.Minus,
                    UnaryOperatorKind.Not => UnOp.Not,
                    _ => null,
                };

                if (op is null) {
                    if (unary.Op is not UnaryOperatorKind.Plus) {
                        Unsupported($"Unary operator '{unary.Op}' is not supported yet.", unary.Location);
                    }

                    return LowerExpression(unary.Operand);
                }

                return new UnaryOperatorExpression {
                    UnOp = op.Value,
                    Expression = LowerExpression(unary.Operand),
                };
            }

            case HirConditional conditional: {
                return LowerConditional(conditional);
            }

            case HirCoalesce coalesce: {
                return LowerCoalesce(coalesce);
            }

            case HirConditionalAccess access: {
                return LowerConditionalAccess(access);
            }

            case HirConditionalReceiver receiver: {
                return LowerConditionalReceiver(receiver);
            }

            case HirImplicitReceiver: {
                return (Expression)_initializerTargets.Peek().DeepClone();
            }

            default: {
                Unsupported($"Lowering '{expression.GetType().Name}' expressions are not supported yet.", expression.Location);

                return Nil();
            }
        }
    }

    private AnonymousFunctionExpression LowerLambda(HirLambda lambda) {
        Log.Verbose("Lowering lambda with {ParameterCount} parameters", lambda.Parameters.Length);

        var parameters = LowerParameters(lambda.Parameters);
        var returnsTuple = IsLuaTuple(lambda.Symbol.ReturnType);
        var returnType = returnsTuple ? TuplePackType() : LowerType(lambda.Symbol.ReturnType, lambda.Location);
        var outerReturnsSelf = _returnsSelf;
        var outerReturnsTuple = _returnsTuple;

        _returnsSelf = false;
        _returnsTuple = returnsTuple;

        var body = LowerBlock(lambda.Body);

        _returnsSelf = outerReturnsSelf;
        _returnsTuple = outerReturnsTuple;

        var function = Lambda(parameters, returnType, body);

        for (var i = 0; i < lambda.Parameters.Length; i++) {
            if (!IsLuaTuple(lambda.Parameters[i].Symbol.Type)) {
                continue;
            }

            if (i != lambda.Parameters.Length - 1) {
                Unsupported("A LuaTuple parameter must be the last parameter of a lambda.", lambda.Parameters[i].Location);

                continue;
            }

            MakeVariadic(function, parameters[i].Name);
        }

        return function;
    }
}
