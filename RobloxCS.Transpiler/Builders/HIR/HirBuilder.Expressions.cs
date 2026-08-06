using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.Transpiler.Builders.HIR;

public sealed partial class HirBuilder {
    public HirExpression? BuildExpression(IOperation operation) {
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

            default: {
                throw new NotSupportedException($"Operation kind '{operation.Kind}' is not supported.");
            }
        }

        return null;

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