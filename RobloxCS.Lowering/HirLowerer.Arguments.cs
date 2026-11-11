using System.Collections.Immutable;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR.Expressions;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression CaptureValue(Expression value, string hint) {
        if (value is NumberExpression or StringExpression or BooleanExpression) {
            return value;
        }

        var name = _names.FreshTemp(hint);
        _names.Emit(Local(name, value));

        return Name(name);
    }

    private List<Expression> LowerArguments(ImmutableArray<HirArgument> arguments) {
        var values = new List<(int Ordinal, Expression Value)>();
        foreach (var argument in arguments) {
            values.Add((argument.Symbol.Ordinal, CaptureValue(LowerExpression(argument.Value), "argument")));
        }

        return [..values.OrderBy(argument => argument.Ordinal).Select(argument => argument.Value)];
    }
}