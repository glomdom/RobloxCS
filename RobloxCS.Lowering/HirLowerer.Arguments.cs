using System.Collections.Immutable;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression CaptureValue(Expression value, string hint) {
        if (value is NumberExpression or StringExpression or BooleanExpression or SymbolExpression { Value: "nil" }) {
            return value;
        }

        var name = _names.FreshTemp(hint);
        _names.Emit(Local(name, value));

        return Name(name);
    }

    private List<Expression> LowerArguments(ImmutableArray<HirArgument> arguments, Action<bool, bool>? prepareReceiver = null, bool materialize = false) {
        var lowered = new List<(int Ordinal, Expression Value, List<Statement> Preludes)>();
        foreach (var argument in arguments) {
            var (value, preludes) = LowerCaptured(argument.Value);

            lowered.Add((argument.Symbol.Ordinal, value, preludes));
        }

        var hasEffects = lowered.Any(argument => HasEvaluationEffects(argument.Value) || argument.Preludes.Count > 0);
        var reordered = !lowered.Select(argument => argument.Ordinal).SequenceEqual(lowered.OrderBy(argument => argument.Ordinal).Select(argument => argument.Ordinal));
        var hoistArguments = (reordered || materialize) && hasEffects;
        prepareReceiver?.Invoke(hoistArguments || lowered.Any(argument => argument.Preludes.Count > 0), hasEffects);

        var lastPrelude = lowered.FindLastIndex(argument => argument.Preludes.Count > 0);
        var values = new List<(int Ordinal, Expression Value)>();

        for (var i = 0; i < lowered.Count; i++) {
            var argument = lowered[i];
            foreach (var prelude in argument.Preludes) {
                _names.Emit(prelude);
            }

            var capture = hoistArguments || i < lastPrelude;
            values.Add((argument.Ordinal, capture ? CaptureValue(argument.Value, "argument") : argument.Value));
        }

        return [..values.OrderBy(argument => argument.Ordinal).Select(argument => argument.Value)];
    }

    private static bool HasEvaluationEffects(AstNode node) {
        if (node is AnonymousFunctionExpression) {
            return false;
        }

        return node is FunctionCallExpression or VarExpression || node.Children().Any(HasEvaluationEffects);
    }
}