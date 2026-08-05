using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.Transpiler.Builders.HIR;

public sealed partial class HirBuilder {
    public HirExpression? BuildExpression(IOperation operation) {
        if (operation.Type is null) {
            throw new InvalidOperationException("Operation provided cannot produce null.");
        }

        switch (operation.Kind) {
            case OperationKind.Literal: {
                var literalOperation = (ILiteralOperation)operation;
                if (literalOperation.ConstantValue.HasValue) {
                    return new HirLiteral {
                        Location = literalOperation.Syntax.GetLocation(),
                        Type = literalOperation.Type!,
                        Value = operation.ConstantValue.Value,
                    };
                }

                break;
            }

            default: {
                throw new NotSupportedException($"Operation kind '{operation.Kind}' is not supported.");
            }
        }

        return null;
    }
}