using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR;

public static class SymbolSlotExtensions {
    public static string InterfaceTypeName(this INamedTypeSymbol type) => "__interfaceType_" + EncodeSlot(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

    public static string SlotName(this IFieldSymbol field) {
        if (field.CapturedPrimaryParameter() is { } parameter) {
            var slotName = "__primary_" + EncodeSlot(field.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)) + "_" + EncodeSlot(parameter.Name);
            while (HasMemberInHierarchy(field.ContainingType, slotName)) {
                slotName += "_";
            }

            return slotName;
        }

        if (field.AssociatedSymbol is IPropertySymbol { ExplicitInterfaceImplementations.Length: > 0 } explicitProperty) {
            return "__property_" + EncodeSlot(explicitProperty.ExplicitInterfaceImplementations[0].ToDisplayString());
        }

        var name = field.AssociatedSymbol is IPropertySymbol property
            ? property.IsAutoProperty() ? property.Name : $"__{property.Name}"
            : field.Name;

        if (!field.IsStatic && HidesBaseMember(field.ContainingType, field.AssociatedSymbol?.Name ?? field.Name)) {
            return $"__{field.ContainingType.Name}_{name}";
        }

        return name;
    }

    public static string SlotName(this IMethodSymbol method) {
        if (method.ContainingType.TypeKind == TypeKind.Interface) {
            return "__interface_" + EncodeSlot(
                                      method.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                  )
                                  + "_"
                                  + EncodeSlot(method.MetadataName)
                                  + "_"
                                  + EncodeSlot(
                                      string.Join(";",
                                          method.Parameters.Select(parameter => parameter.RefKind + ":" + parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                                      )
                                  );
        }

        if (!method.ExplicitInterfaceImplementations.IsEmpty) {
            return method.ExplicitInterfaceImplementations[0].SlotName();
        }

        if (method.OverriddenMethod is { } overridden) {
            return overridden.SlotName();
        }

        if (!method.IsStatic && HidesBaseMember(method.ContainingType, method.AssociatedSymbol?.Name ?? method.Name)) {
            return $"__{method.ContainingType.Name}_{method.Name}";
        }

        return method.Name;
    }

    private static string EncodeSlot(string value) =>
        string.Concat(value.Select(character => char.IsAsciiLetterOrDigit(character) ? character.ToString() : $"_{(int)character:x4}"));

    private static bool HasMemberInHierarchy(INamedTypeSymbol type, string name) {
        for (var current = type; current is not null; current = current.BaseType) {
            if (current.GetMembers(name).Length > 0) {
                return true;
            }
        }

        return false;
    }

    private static bool HidesBaseMember(INamedTypeSymbol type, string name) {
        for (var current = type.BaseType; current is { SpecialType: not SpecialType.System_Object }; current = current.BaseType) {
            if (current.GetMembers(name).Length > 0) {
                return true;
            }
        }

        return false;
    }
}