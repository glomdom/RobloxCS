using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirExpression BuildExpression(IOperation operation) {
        switch (operation.Kind) {
            case OperationKind.Literal when operation is ILiteralOperation literalOperation: {
                return HandleLiteralOperation(literalOperation);
            }

            case OperationKind.VariableInitializer when operation is IVariableInitializerOperation variableInitializerOperation: {
                return BuildExpression(variableInitializerOperation.Value);
            }

            case OperationKind.LocalReference when operation is ILocalReferenceOperation localReferenceOperation: {
                if (localReferenceOperation.IsDeclaration) {
                    throw new NotSupportedException("in/out references are not supported.");
                }

                if (localReferenceOperation.Type is not { } type) {
                    throw new InvalidOperationException("Local reference has a null type.");
                }

                return new HirLocalRef {
                    Location = localReferenceOperation.Syntax.GetLocation(),
                    Symbol = localReferenceOperation.Local,
                    Type = type,
                };
            }

            case OperationKind.ObjectCreation when operation is IObjectCreationOperation creationOperation: {
                if (creationOperation.Type is not { } type) {
                    throw new InvalidOperationException("Object creation resulted in null type.");
                }

                if (creationOperation.Constructor is not { } ctor) {
                    throw new InvalidOperationException("Object creation called without a ctor.");
                }

                var args = creationOperation.Arguments.Select(BuildArgument).ToImmutableArray();

                return new HirObjectCreation {
                    Location = creationOperation.Syntax.GetLocation(),
                    Type = type,
                    Constructor = ctor,
                    Arguments = args,
                };
            }

            case OperationKind.ParameterReference when operation is IParameterReferenceOperation parameterReferenceOperation: {
                if (parameterReferenceOperation.Type is not { } type) {
                    throw new InvalidOperationException("Parameter reference type is null.");
                }

                return new HirParameterRef {
                    Location = parameterReferenceOperation.Syntax.GetLocation(),
                    Type = type,
                    Symbol = parameterReferenceOperation.Parameter,
                };
            }

            case OperationKind.Binary when operation is IBinaryOperation binaryOperation: {
                if (binaryOperation.Type is not { } type) {
                    throw new InvalidOperationException("Binary operation does not produce a type.");
                }

                if (binaryOperation.IsLifted) {
                    throw new NotSupportedException("Lifted binary operations are not supported.");
                }

                var left = BuildExpression(binaryOperation.LeftOperand);
                var right = BuildExpression(binaryOperation.RightOperand);

                return new HirBinary {
                    Location = binaryOperation.Syntax.GetLocation(),
                    Type = type,
                    IsChecked = binaryOperation.IsChecked,
                    Left = left,
                    Right = right,
                    Op = binaryOperation.OperatorKind,
                    OperatorMethod = binaryOperation.OperatorMethod,
                };
            }

            case OperationKind.Conversion when operation is IConversionOperation conversionOperation: {
                if (conversionOperation.IsTryCast) {
                    throw new NotSupportedException("Try casts are not supported yet.");
                }

                if (conversionOperation.Type is not { } type) {
                    throw new InvalidOperationException("Conversion operation does not produce a type.");
                }

                var conversion = conversionOperation.GetConversion();

                return new HirConversion {
                    Location = conversionOperation.Syntax.GetLocation(),
                    Type = conversionOperation.Type,
                    Operand = BuildExpression(conversionOperation.Operand),
                    Kind = ClassifyConversion(conversion, type),
                    Method = conversionOperation.OperatorMethod,
                    IsImplicit = conversion.IsImplicit,
                };
            }

            case OperationKind.FieldReference when operation is IFieldReferenceOperation fieldReferenceOperation: {
                if (fieldReferenceOperation.Type is not { } type) {
                    throw new InvalidOperationException("Field reference operation does not have a type.");
                }

                var receiver = fieldReferenceOperation.Field.IsStatic ? null : BuildExpression(fieldReferenceOperation.Instance!);

                return new HirFieldAccess {
                    Location = fieldReferenceOperation.Syntax.GetLocation(),
                    Type = type,
                    Symbol = fieldReferenceOperation.Field,
                    Receiver = receiver,
                };
            }

            case OperationKind.InstanceReference when operation is IInstanceReferenceOperation instanceReferenceOperation: {
                if (instanceReferenceOperation.ReferenceKind == InstanceReferenceKind.ContainingTypeInstance) {
                    return new HirThis {
                        Location = instanceReferenceOperation.Syntax.GetLocation(),
                        Type = instanceReferenceOperation.Type!,
                    };
                }

                if (instanceReferenceOperation.Syntax is BaseExpressionSyntax) {
                    throw new NotSupportedException("base is not supported.");
                }

                throw new NotSupportedException($"Instance reference of kind '{instanceReferenceOperation.ReferenceKind}' is not supported yet.");
            }

            case OperationKind.Invocation when operation is IInvocationOperation invocationOperation: {
                if (invocationOperation.Type is not { } type) {
                    throw new InvalidOperationException("Invocation operation does not have a type.");
                }

                var receiver = invocationOperation.Instance is not null ? BuildExpression(invocationOperation.Instance) : null;
                var args = invocationOperation.Arguments.Select(BuildArgument).ToImmutableArray();
                
                return new HirCall {
                    Location = invocationOperation.Syntax.GetLocation(),
                    Type = type,
                    Method = invocationOperation.TargetMethod,
                    Receiver = receiver,
                    Arguments = args,
                    IsExtension = false,
                };
            }

            case OperationKind.PropertyReference when operation is IPropertyReferenceOperation propertyReferenceOperation: {
                if (propertyReferenceOperation.Type is not { } type) {
                    throw new InvalidOperationException("Property reference operation does not have a type.");
                }

                var receiver = propertyReferenceOperation.Instance is not null ? BuildExpression(propertyReferenceOperation.Instance) : null;
                
                return new HirPropertyAccess {
                    Location = propertyReferenceOperation.Syntax.GetLocation(),
                    Type = type,
                    Receiver = receiver,
                    Symbol = propertyReferenceOperation.Property,
                };
            }

            case OperationKind.DelegateCreation when operation is IDelegateCreationOperation delegateCreationOperation: {
                if (delegateCreationOperation.Type is not { } delegateType) {
                    throw new InvalidOperationException("Delegate creation operation does not produce a type.");
                }

                switch (delegateCreationOperation.Target) {
                    case IAnonymousFunctionOperation anon: {
                        if (anon.Symbol.IsAsync) {
                            throw new NotSupportedException("Async lambdas are not supported yet.");
                        }

                        return new HirLambda {
                            Location = anon.Syntax.GetLocation(),
                            Type = delegateType,
                            Symbol = anon.Symbol,
                            Parameters = [.. anon.Symbol.Parameters.Select(BuildParameter)],
                            Body = BuildBlock(anon.Body),
                        };
                    }

                    case IMethodReferenceOperation: {
                        throw new NotSupportedException("Passing a method group as a delegate is not supported yet.");
                    }

                    default: {
                        throw new NotSupportedException($"Delegate creation with target '{delegateCreationOperation.Target.Kind}' is not supported.");
                    }
                }
            }

            case OperationKind.Conditional when operation is IConditionalOperation conditionalOperation: {
                if (conditionalOperation.IsRef) {
                    throw new NotSupportedException("ref conditional expressions are not supported.");
                }

                if (conditionalOperation.WhenFalse is not { } whenFalse || conditionalOperation.Type is not { } type) {
                    throw new InvalidOperationException("Conditional expression has no else branch or no type.");
                }

                return new HirConditional {
                    Location = conditionalOperation.Syntax.GetLocation(),
                    Type = type,
                    Condition = BuildExpression(conditionalOperation.Condition),
                    WhenTrue = BuildExpression(conditionalOperation.WhenTrue),
                    WhenFalse = BuildExpression(whenFalse),
                };
            }

            default: {
                throw new NotSupportedException($"Expression operation kind '{operation.Kind}' is not supported.");
            }
        }

        HirLiteral HandleLiteralOperation(ILiteralOperation literalOperation) {
            var constant = literalOperation.ConstantValue;

            return new HirLiteral {
                Location = literalOperation.Syntax.GetLocation(),
                Type = literalOperation.Type!,
                Value = constant.HasValue ? constant.Value : null,
            };
        }
    }

    private static HirConversionKind ClassifyConversion(Conversion conversion, ITypeSymbol target) {
        if (conversion.IsUserDefined) {
            return HirConversionKind.UserDefined;
        }

        if (conversion.IsIdentity) {
            return HirConversionKind.Identity;
        }

        if (conversion.IsBoxing) {
            return HirConversionKind.Boxing;
        }

        if (conversion.IsUnboxing) {
            return HirConversionKind.Unboxing;
        }

        if (conversion.IsNullable) {
            return target.OriginalDefinition.SpecialType is SpecialType.System_Nullable_T
                ? HirConversionKind.NullableWrap
                : HirConversionKind.NullableUnwrap;
        }

        if (conversion.IsNumeric || conversion.IsConstantExpression) {
            return HirConversionKind.Numeric;
        }

        if (conversion.IsReference || conversion.IsNullLiteral) {
            return HirConversionKind.Reference;
        }

        throw new NotSupportedException($"Conversion '{conversion}' is not supported yet.");
    }
}