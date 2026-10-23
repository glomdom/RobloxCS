using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirArgument BuildArgument(IArgumentOperation operation) {
        if (operation.Parameter is null) {
            throw new NotSupportedException("__arglist is not supported");
        }

        // TODO: ArgumentKind.DefaultValue: the value is a synthesized default and currently goes through BuildExpression like any other.
        var value = BuildExpression(operation.Value);

        return new HirArgument {
            Symbol = operation.Parameter,
            Location = operation.Syntax.GetLocation(),
            Value = value,
            // IsParams = argumentOperation.Parameter.IsParams,
            // RefKind = argumentOperation.Parameter.RefKind,
        };
    }
}
