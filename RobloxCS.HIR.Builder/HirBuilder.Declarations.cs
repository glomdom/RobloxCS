using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.HIR.Declarations;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirType BuildType(INamedTypeSymbol typeSymbol, BaseTypeDeclarationSyntax typeSyntax) {
        if (typeSymbol.TypeKind == TypeKind.Enum) {
            return BuildEnum(typeSymbol);
        }

        if (typeSymbol.TypeKind == TypeKind.Interface) {
            return new HirType {
                Location = typeSymbol.Locations.ResolveSingle(), Symbol = typeSymbol, Base = null,
                Fields = [], Methods = [], Properties = [],
            };
        }

        var methods = new List<HirMethod>();
        var fields = new List<HirField>();

        foreach (var member in typeSymbol.GetMembers()) {
            switch (member) {
                case IFieldSymbol { AssociatedSymbol: IPropertySymbol owner } backing when owner.Kind() is PropertyKind.SemiAuto: {
                    fields.Add(BuildBackingField(backing, owner));

                    break;
                }

                case IFieldSymbol { AssociatedSymbol: IPropertySymbol }: {
                    break;
                }

                case IPropertySymbol { IsIndexer: false } propertySymbol when propertySymbol.IsAutoProperty(): {
                    fields.Add(BuildAutoProperty(propertySymbol));

                    break;
                }

                case IMethodSymbol { MethodKind: MethodKind.PropertyGet or MethodKind.PropertySet, AssociatedSymbol: IPropertySymbol owner }
                    when owner.IsAutoProperty() && owner is { IsVirtual: false, IsOverride: false }: {
                    break;
                }

                case IMethodSymbol { MethodKind: MethodKind.PropertyGet or MethodKind.PropertySet } accessor when !accessor.IsAbstract && !accessor.HasAccessorBody(): {
                    methods.Add(BuildSynthesizedAccessor(accessor));

                    break;
                }

                case IFieldSymbol fieldSymbol: {
                    var field = BuildField(fieldSymbol);
                    fields.Add(field);

                    break;
                }

                // Roslyn synthesizes this
                case IMethodSymbol { MethodKind: MethodKind.StaticConstructor, IsImplicitlyDeclared: true }: {
                    break;
                }

                case IMethodSymbol methodSymbol: {
                    var method = BuildMethod(methodSymbol);
                    methods.Add(method);

                    break;
                }
            }
        }

        return new HirType {
            Location = typeSymbol.Locations.ResolveSingle(),
            Symbol = typeSymbol,
            Base = typeSymbol.BaseType,
            Fields = [..fields],
            Methods = [..methods],
            Properties = [],
        };
    }
}