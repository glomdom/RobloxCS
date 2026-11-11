using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirField BuildAutoProperty(IPropertySymbol property) {
        var syntax = Semantics.GetFirstSyntaxFromSymbol<PropertyDeclarationSyntax>(property);

        HirExpression? initializer = null;
        if (syntax.Initializer is not null) {
            var operation = Semantics.CheckedGetOperation<IPropertyInitializerOperation>(syntax.Initializer);

            initializer = BuildExpression(operation.Value);
        }

        return new HirField {
            Location = property.Locations.ResolveSingle(),
            Symbol = property.BackingField()!,
            Name = property.Name,
            Initializer = initializer,
            IsStatic = property.IsStatic,
        };
    }

    private HirField BuildBackingField(IFieldSymbol backing, IPropertySymbol property) {
        var syntax = Semantics.GetFirstSyntaxFromSymbol<PropertyDeclarationSyntax>(property);

        HirExpression? initializer = null;
        if (syntax.Initializer is not null) {
            var operation = Semantics.CheckedGetOperation<IPropertyInitializerOperation>(syntax.Initializer);

            initializer = BuildExpression(operation.Value);
        }

        return new HirField {
            Location = property.Locations.ResolveSingle(),
            Symbol = backing,
            Name = backing.SlotName(),
            Initializer = initializer,
            IsStatic = property.IsStatic,
        };
    }

    private HirMethod BuildSynthesizedAccessor(IMethodSymbol accessor) {
        var property = (IPropertySymbol)accessor.AssociatedSymbol!;
        var backing = property.BackingField()!;
        var location = accessor.Locations.ResolveSingle();

        HirExpression? receiver = property.IsStatic ? null : new HirThis { Location = location, Type = property.ContainingType };
        var slot = new HirFieldAccess { Location = location, Type = backing.Type, Symbol = backing, Receiver = receiver };

        HirStatement statement = accessor.MethodKind is MethodKind.PropertyGet
            ? new HirReturn { Location = location, Value = slot }
            : new HirAssignment {
                Location = location,
                Target = slot,
                Value = new HirParameterRef { Location = location, Type = accessor.Parameters[0].Type, Symbol = accessor.Parameters[0] },
            };

        return new HirMethod {
            Location = location,
            Symbol = accessor,
            Parameters = [.. accessor.Parameters.Select(BuildParameter)],
            TypeParameters = [],
            Block = new HirBlock { Location = location, Statements = [statement], Locals = [] },
            IsStatic = accessor.IsStatic,
            IsConstructor = false,
            IsEntryPoint = false,
            IsImplicit = true,
        };
    }
}
