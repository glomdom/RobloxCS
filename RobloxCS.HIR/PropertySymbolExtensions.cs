using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxCS.HIR;

public enum PropertyKind {
    Auto,
    SemiAuto,
    Computed,
}

public static class PropertySymbolExtensions {
    extension(IPropertySymbol property) {
        public IFieldSymbol? BackingField() {
            return property.ContainingType
                .GetMembers()
                .OfType<IFieldSymbol>()
                .FirstOrDefault(field => SymbolEqualityComparer.Default.Equals(field.AssociatedSymbol, property));
        }

        public PropertyKind Kind() {
            if (property.BackingField() is null) {
                return PropertyKind.Computed;
            }

            var hasBody = property.GetMethod?.HasAccessorBody() == true || property.SetMethod?.HasAccessorBody() == true;

            return hasBody ? PropertyKind.SemiAuto : PropertyKind.Auto;
        }

        public bool IsAutoProperty() => property.Kind() is PropertyKind.Auto;
        public bool IsSourceProperty() => !property.DeclaringSyntaxReferences.IsEmpty;
    }

    public static bool HasAccessorBody(this IMethodSymbol accessor) {
        return accessor.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() switch {
            AccessorDeclarationSyntax syntax => syntax.Body is not null || syntax.ExpressionBody is not null,
            ArrowExpressionClauseSyntax => true,

            _ => false,
        };
    }

    public static string SlotName(this IFieldSymbol field) {
        var name = field.AssociatedSymbol is IPropertySymbol property
            ? property.IsAutoProperty() ? property.Name : $"__{property.Name}"
            : field.Name;

        if (!field.IsStatic && HidesBaseMember(field.ContainingType, field.AssociatedSymbol?.Name ?? field.Name)) {
            return $"__{field.ContainingType.Name}_{name}";
        }

        return name;
    }

    public static string SlotName(this IMethodSymbol method) {
        if (method.OverriddenMethod is { } overridden) {
            return overridden.SlotName();
        }

        if (!method.IsStatic && HidesBaseMember(method.ContainingType, method.AssociatedSymbol?.Name ?? method.Name)) {
            return $"__{method.ContainingType.Name}_{method.Name}";
        }

        return method.Name;
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
