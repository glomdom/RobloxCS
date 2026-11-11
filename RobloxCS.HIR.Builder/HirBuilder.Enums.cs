using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    private HirType BuildEnum(INamedTypeSymbol typeSymbol) {
        return new HirType {
            Location = typeSymbol.Locations.ResolveSingle(),
            Symbol = typeSymbol,
            Base = typeSymbol.BaseType,
            Fields = [.. typeSymbol.GetMembers().OfType<IFieldSymbol>().Where(field => field.HasConstantValue).Select(field => new HirField {
                Location = field.Locations.ResolveSingle(),
                Symbol = field,
                Name = field.Name,
                IsStatic = true,
                Initializer = new HirLiteral { Location = field.Locations.ResolveSingle(), Type = typeSymbol, Value = field.ConstantValue },
            })],
            Methods = [],
            Properties = [],
        };
    }
}
