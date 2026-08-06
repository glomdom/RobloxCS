using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using Serilog;

namespace RobloxCS.Transpiler.Builders.HIR;

public sealed partial class HirBuilder {
    public HirExpression BuildExpression(IOperation operation) {
        switch (operation.Kind) {
            case OperationKind.Literal when operation is ILiteralOperation literalOperation: {
                return HandleLiteralOperation(literalOperation);
            }

            case OperationKind.VariableInitializer when operation is IVariableInitializerOperation variableInitializerOperation: {
                if (variableInitializerOperation.Value is ILiteralOperation literalValue) {
                    return HandleLiteralOperation(literalValue);
                }

                throw new NotSupportedException($"Variable initializer operation with initializer '{variableInitializerOperation.Value.Kind}' is not supported.");
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

            case OperationKind.Argument when operation is IArgumentOperation argumentOperation: {
                if (argumentOperation.Type is not null) {
                    Log.Information("Argument has constant value {ConstantValue}", argumentOperation.Value.ConstantValue.HasValue);
                }

                if (argumentOperation.Parameter is null) {
                    throw new NotSupportedException("__arglist is not supported");
                }

                HirExpression? defaultValue = null;
                if (argumentOperation.ArgumentKind == ArgumentKind.DefaultValue) {
                    Log.Information("TODO");
                }

                var value = BuildExpression(argumentOperation.Value);

                return new HirArgument {
                    Symbol = argumentOperation.Parameter,
                    Location = argumentOperation.Syntax.GetLocation(),
                    Type = argumentOperation.Type!,
                    Value = value,
                    // IsParams = argumentOperation.Parameter.IsParams,
                    // RefKind = argumentOperation.Parameter.RefKind,
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

            default: {
                throw new NotSupportedException($"Operation kind '{operation.Kind}' is not supported.");
            }
        }

        HirLiteral HandleLiteralOperation(ILiteralOperation literalOperation) {
            if (literalOperation.ConstantValue.HasValue) {
                return new HirLiteral {
                    Location = literalOperation.Syntax.GetLocation(),
                    Type = literalOperation.Type!,
                    Value = literalOperation.ConstantValue.Value,
                };
            }

            Console.WriteLine(literalOperation.Kind);

            return new HirLiteral {
                Location = literalOperation.Syntax.GetLocation(),
                Type = literalOperation.Type!,
                Value = null,
            };
        }
    }
}