using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private static readonly HashSet<string> LuauKeywords = [
        "and", "break", "do", "else", "elseif", "end", "false", "for", "function", "if", "in",
        "local", "nil", "not", "or", "repeat", "return", "then", "true", "until", "while",
    ];

    private readonly HashSet<HirExpression> _nativeTables = new(ReferenceEqualityComparer.Instance);

    private HirExpression MarkNativeTable(HirExpression value) {
        var literal = value;
        while (literal is HirConversion { Kind: HirConversionKind.Identity } conversion) {
            literal = conversion.Operand;
        }

        if (IsNativeTableLiteral(literal)) {
            _nativeTables.Add(literal);
        }

        return value;
    }

    private static bool IsNativeTableLiteral(HirExpression value) {
        return value switch {
            HirCollectionLiteral literal => IsExpandedParams(literal),
            HirObjectCreation { Arguments.IsEmpty: true } creation => (IsList(creation.Type) || IsDictionary(creation.Type)) &&
                                                                      creation.Initializers.All(initializer => TableEntry(initializer) is not null),

            _ => false,
        };
    }

    private static bool IsDictionary(ITypeSymbol type) {
        return type.OriginalDefinition.ToDisplayString() == "System.Collections.Generic.Dictionary<TKey, TValue>";
    }

    private static (HirExpression? Key, HirExpression Value)? TableEntry(HirStatement initializer) {
        return initializer switch {
            HirExpressionStatement { Expression: HirCall { Method.Name: "Add", Arguments: [var item] } } => (null, item.Value),
            HirExpressionStatement { Expression: HirCall { Method.Name: "Add", Arguments: [var key, var item] } } => (key.Value, item.Value),
            HirAssignment { Target: HirIndexAccess { Indices: [var key] }, Value: var item } => (key, item),

            _ => null,
        };
    }

    private Expression LowerNativeTable(HirExpression value) {
        var entries = value switch {
            HirCollectionLiteral literal => literal.Elements.Select(element => ((HirExpression?)null, element)).ToList(),
            HirObjectCreation creation => creation.Initializers.Select(initializer => TableEntry(initializer)!.Value).ToList(),

            _ => [],
        };

        var producers = new List<Func<Expression>>();
        foreach (var (key, item) in entries) {
            if (key is not null && TableName(key) is null) {
                producers.Add(() => LowerNativeValue(key));
            }

            producers.Add(() => LowerNativeValue(item));
        }

        var lowered = LowerOrdered(producers);
        var fields = new List<TableField>();
        var next = 0;

        foreach (var (key, _) in entries) {
            if (key is null) {
                fields.Add(new NoKey { Expression = lowered[next++] });
            } else if (TableName(key) is { } name) {
                fields.Add(new NameKey { Key = name, Value = lowered[next++] });
            } else {
                var keyValue = lowered[next++];
                fields.Add(new IndexKey { Key = keyValue, Value = lowered[next++] });
            }
        }

        return TableConstructorExpression.With(fields);
    }

    private Expression LowerNativeValue(HirExpression value) {
        var raw = UnwrapBox(value);

        return IsNativeTableLiteral(raw) ? LowerNativeTable(raw) : LowerExpression(raw);
    }

    private static string? TableName(HirExpression key) {
        return UnwrapBox(key) is HirLiteral { Value: string name } && Regex.IsMatch(name, "^[A-Za-z_][A-Za-z0-9_]*$") && !LuauKeywords.Contains(name)
            ? name
            : null;
    }

    private List<Expression> LowerOrdered(List<Func<Expression>> producers) {
        var lowered = producers.Select(producer => {
            Expression value = Nil();
            var preludes = _names.Capture(() => value = producer());

            return (Value: value, Preludes: preludes);
        }).ToList();

        var lastPrelude = lowered.FindLastIndex(entry => entry.Preludes.Count > 0);
        var values = new List<Expression>();

        for (var i = 0; i < lowered.Count; i++) {
            foreach (var prelude in lowered[i].Preludes) {
                _names.Emit(prelude);
            }

            values.Add(i < lastPrelude && HasEvaluationEffects(lowered[i].Value) ? CaptureValue(lowered[i].Value, "value") : lowered[i].Value);
        }

        return values;
    }
}
