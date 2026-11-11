using Microsoft.CodeAnalysis;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Declarations;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private List<Statement> LowerTypeDeclaration(HirType type) {
        switch (type.Symbol.TypeKind) {
            case TypeKind.Class: return LowerClass(type);
            case TypeKind.Enum: return LowerEnum(type);

            default: {
                Unsupported("Only classes and enums are supported as top-level types.", type.Location);

                return [];
            }
        }
    }
}
