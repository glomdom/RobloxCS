using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;
using Serilog;

namespace RobloxCS.Transpiler.Builders.HIR;

public partial class HirBuilder {
    public HirArgument BuildArgument(IArgumentOperation operation) {
        if (operation.Type is not null) {
            Log.Information("Argument has constant value {ConstantValue}", operation.Value.ConstantValue.HasValue);
        }

        if (operation.Parameter is null) {
            throw new NotSupportedException("__arglist is not supported");
        }

        if (operation.ArgumentKind == ArgumentKind.DefaultValue) {
            Log.Information("TODO");
        }

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