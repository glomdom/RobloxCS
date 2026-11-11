using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using Serilog;
using static RobloxCS.Lowering.Luau;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private TypeInfo LowerFieldType(HirField field) {
        var type = LowerType(field.Symbol.Type, field.Location);
        var startsNil = field.Initializer is null && field.Symbol.Type.IsReferenceType;

        if (startsNil || field.Symbol.NullableAnnotation is NullableAnnotation.Annotated) {
            Log.Verbose("Field {FieldName} can be nil, typing it as optional", field.Symbol.Name);

            return Optional(type);
        }

        return type;
    }

    private Expression LowerFieldValue(HirField field) {
        if (field.Initializer is { } initializer) {
            return LowerExpression(initializer);
        }

        return DefaultValue(field.Symbol.Type);
    }

    private static Expression DefaultValue(ITypeSymbol type) {
        if (type.IsReferenceType || type.OriginalDefinition.SpecialType is SpecialType.System_Nullable_T) {
            return Nil();
        }

        return type.SpecialType switch {
            SpecialType.System_Boolean => Bool(false),
            SpecialType.System_Char => Str("\0"),

            _ => Num(0),
        };
    }

    private VarExpression LowerFieldAccess(HirFieldAccess access) {
        var field = access.Symbol;

        if (field.IsStatic && IsRobloxEnum(field.ContainingType)) {
            return Index(Index("Enum", field.ContainingType.Name), field.Name);
        }

        if (field.ContainingType.TypeKind == TypeKind.Enum) {
            if (!IsSourceEnum(field.ContainingType)) {
                Unsupported($"Enum '{field.ContainingType.ToDisplayString()}' is not declared in a transpiled module.", access.Location);

                return Index("__unsupportedEnum", field.Name);
            }

            return Index(ReferenceType(field.ContainingType, access.Location), field.Name);
        }

        if (field.IsStatic) {
            Log.Verbose("Lowering static field access {ClassName}.{FieldName}", field.ContainingType.Name, field.SlotName());

            return Index(ReferenceType(field.ContainingType, access.Location), field.SlotName());
        }

        var receiver = access.Receiver is null ? Name("self") : LowerExpression(access.Receiver);

        return Index(receiver, field.SlotName());
    }
}
