using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirType BuildType(INamedTypeSymbol typeSymbol, TypeDeclarationSyntax typeSyntax) {
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

                case IMethodSymbol { MethodKind: MethodKind.PropertyGet or MethodKind.PropertySet, AssociatedSymbol: IPropertySymbol owner } when owner.IsAutoProperty(): {
                    break;
                }
                
                case IMethodSymbol { MethodKind: MethodKind.PropertyGet or MethodKind.PropertySet } accessor when !accessor.HasAccessorBody(): {
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
            Fields = [.. fields],
            Methods = [.. methods],
            Properties = [],
        };
    }
    
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

    public HirField BuildField(IFieldSymbol field) {
        if (field.IsImplicitlyDeclared) {
            Log.Verbose("Skipping {FieldName} as it is implicitly declared", field.Name);
        }

        var syntax = Semantics.GetFirstSyntaxFromSymbol<VariableDeclaratorSyntax>(field);

        HirExpression? initializer = null;
        if (syntax.Initializer is not null) {
            var operation = Semantics.CheckedGetOperation<IFieldInitializerOperation>(syntax.Initializer);

            initializer = BuildExpression(operation.Value);
        }

        return new HirField {
            Location = field.Locations.ResolveSingle(),
            Symbol = field,
            Initializer = initializer,
            Name = field.Name,
            IsStatic = field.IsStatic,
        };
    }

    public HirMethod BuildMethod(IMethodSymbol method) {
        Log.Verbose("Adding method {MethodName} of kind {MethodKind}", method.Name, method.MethodKind);

        var isCtor = method is { MethodKind: MethodKind.Constructor };
        var isImplicitCtor = method is { IsImplicitlyDeclared: true } && isCtor;
        var isEntryPoint = IsEntryPoint(method);

        var parameters = method.Parameters.Select(BuildParameter).ToList();
        var block = isImplicitCtor ? BuildImplicitConstructorBlock(method) : BuildMethodBlock(method);

        return new HirMethod {
            Location = method.Locations.ResolveSingle(),
            Symbol = method,
            Parameters = [.. parameters],
            TypeParameters = [], // todo
            Block = block,
            IsStatic = method.IsStatic,
            IsConstructor = isCtor,
            IsEntryPoint = isEntryPoint,
            IsImplicit = method is { IsImplicitlyDeclared: true },
        };
    }

    private bool IsEntryPoint(IMethodSymbol method) {
        var entryPointAttr = Compiler.Types.EntryPointAttributeSymbol;

        return method.GetAttributes().Any(attr => SymbolEqualityComparer.Default.Equals(attr.AttributeClass, entryPointAttr));
    }

    private HirBlock BuildMethodBlock(IMethodSymbol method) {
        var syntax = method.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();

        var body = syntax switch {
            BaseMethodDeclarationSyntax or AccessorDeclarationSyntax => Semantics.GetOperation(syntax) is IMethodBodyBaseOperation operation
                ? operation.BlockBody ?? operation.ExpressionBody
                : null,
            ArrowExpressionClauseSyntax arrow => Semantics.GetOperation(arrow) as IBlockOperation,
            _ => null,
        };

        if (body is null) {
            var error = Unsupported("this kind of method body is not supported", method.Locations.ResolveSingle());

            return new HirBlock { Location = Location.None, Statements = [error], Locals = [] };
        }

        return new HirBlock {
            Location = Location.None,
            Statements = [.. body.Operations.Select(BuildStatement)],
            Locals = [.. body.Locals],
        };
    }

    private HirBlock BuildImplicitConstructorBlock(IMethodSymbol method) {
        Log.Debug("Reconstructing implicit constructor {MethodName}", method.Name);

        if (method.DeclaringSyntaxReferences.FirstOrDefault() is not null) {
            throw new InvalidOperationException("Implicit constructor contains a declaring syntax.");
        }

        if (method.ContainingSymbol is not INamedTypeSymbol cls) {
            throw new NotSupportedException("Reconstructing implicit constructor is not supported for types other than classes.");
        }

        var statements = new List<HirStatement>();
        var locals = new List<ILocalSymbol>();

        foreach (var fieldSymbol in cls.GetMembers().OfType<IFieldSymbol>()) {
            if (fieldSymbol.IsStatic || fieldSymbol.IsConst || fieldSymbol.IsImplicitlyDeclared) {
                continue;
            }

            var syntax = Semantics.GetFirstSyntaxFromSymbol<VariableDeclaratorSyntax>(fieldSymbol);
            if (syntax.Initializer is null) {
                continue;
            }

            var fieldOperation = Semantics.CheckedGetOperation<IFieldInitializerOperation>(syntax.Initializer);

            Log.Verbose("Added field {FieldSymbolName} to synthesized ctor ({LocalCount} locals)", fieldSymbol.Name, fieldOperation.Locals.Length);

            locals.AddRange(fieldOperation.Locals);
            statements.Add(BuildStatement(fieldOperation));
        }

        return new HirBlock { Location = Location.None, Statements = [.. statements], Locals = [.. locals] };
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
