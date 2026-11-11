using Microsoft.CodeAnalysis;
using static RobloxCS.Lowering.Luau;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private TypeInfo LowerType(ITypeSymbol type, Location location) {
        if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable) {
            return Optional(LowerType(nullable.TypeArguments[0], location));
        }

        if (type is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated }) {
            return Optional(LowerType(type.WithNullableAnnotation(NullableAnnotation.NotAnnotated), location));
        }

        switch (type.SpecialType) {
            case SpecialType.System_Void: {
                return VoidType();
            }

            case SpecialType.System_Boolean: {
                return Type("boolean");
            }

            case SpecialType.System_String:
            case SpecialType.System_Char: {
                return Type("string");
            }

            case SpecialType.System_SByte:
            case SpecialType.System_Byte:
            case SpecialType.System_Int16:
            case SpecialType.System_UInt16:
            case SpecialType.System_Int32:
            case SpecialType.System_UInt32:
            case SpecialType.System_Int64:
            case SpecialType.System_UInt64:
            case SpecialType.System_Single:
            case SpecialType.System_Double:
            case SpecialType.System_Decimal: {
                return Type("number");
            }
        }

        if (GetRobloxNative(type) is { } native) {
            return Type(native.Name);
        }

        if (IsSourceEnum(type)) {
            return Type("number");
        }

        if (IsTranspiledType(type)) {
            return Type(ReferenceType((INamedTypeSymbol)type, location));
        }

        if (IsRobloxEnum(type)) {
            return Type($"Enum.{type.Name}");
        }

        Unsupported($"Type '{type.ToDisplayString()}' is not supported yet.", location);

        return Type("any");
    }
}
