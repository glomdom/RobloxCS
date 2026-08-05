using Microsoft.CodeAnalysis;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.Transpiler.Builders.HIR;

public sealed partial class HirBuilder {
    public HirParameter BuildParameter(IParameterSymbol parameterSymbol) {
        HirExpression? defaultValue = null;

        if (parameterSymbol.HasExplicitDefaultValue) {
            if (parameterSymbol.ExplicitDefaultValue is { } value) {
                defaultValue = new HirLiteral {
                    Location = Location.None,
                    Type = parameterSymbol.Type,
                    Value = value,
                };
            }
        }

        return new HirParameter {
            Location = SyntaxUtilities.ResolveLocations(parameterSymbol.Locations),
            Symbol = parameterSymbol,
            DefaultValue = defaultValue,
            IsParams = parameterSymbol.IsParams,
            RefKind = parameterSymbol.RefKind,
        };
    }
}