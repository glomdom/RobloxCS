using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirArgument BuildArgument(IArgumentOperation operation) {
        if (operation.Parameter is null) {
            throw new NotSupportedException("__arglist is not supported");
        }

        var value = BuildExpression(operation.Value);

        return new HirArgument {
            Symbol = operation.Parameter,
            Location = operation.Syntax.GetLocation(),
            Value = value,
            IsDefault = operation.ArgumentKind == ArgumentKind.DefaultValue,
            // IsParams = argumentOperation.Parameter.IsParams,
            // RefKind = argumentOperation.Parameter.RefKind,
        };
    }
}
