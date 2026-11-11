using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxCS.HIR;

public static class PrimaryConstructorSymbolExtensions {
    public static bool IsPrimaryConstructor(this IMethodSymbol method) =>
        method.MethodKind == MethodKind.Constructor && method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() is ClassDeclarationSyntax { ParameterList: not null };

    public static IParameterSymbol? CapturedPrimaryParameter(this IFieldSymbol field) {
        if (field is not { IsImplicitlyDeclared: true, AssociatedSymbol: null }) {
            return null;
        }

        return field.ContainingType.InstanceConstructors
            .Where(constructor => constructor.IsPrimaryConstructor())
            .SelectMany(constructor => constructor.Parameters)
            .FirstOrDefault(parameter => field.Name == $"<{parameter.Name}>P");
    }

    public static IFieldSymbol? PrimaryParameterField(this IParameterSymbol parameter) {
        if (parameter.ContainingSymbol is not IMethodSymbol method || !method.IsPrimaryConstructor()) {
            return null;
        }

        return method.ContainingType.GetMembers($"<{parameter.Name}>P").OfType<IFieldSymbol>().FirstOrDefault(field => field.IsImplicitlyDeclared);
    }
}