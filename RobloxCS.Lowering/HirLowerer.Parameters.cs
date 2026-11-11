using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.HIR;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Var LowerParameterReference(IParameterSymbol parameter) {
        if (!SymbolEqualityComparer.Default.Equals(parameter.ContainingSymbol, _initializingConstructor) && parameter.PrimaryParameterField() is { } field) {
            return Index("self", field.SlotName());
        }

        return VarName.FromString(_names.NameFor(parameter));
    }
}