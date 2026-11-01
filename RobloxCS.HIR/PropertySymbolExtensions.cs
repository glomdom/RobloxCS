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
        if (field.AssociatedSymbol is not IPropertySymbol property) {
            return field.Name;
        }

        return property.IsAutoProperty() ? property.Name : $"__{property.Name}";
    }
}