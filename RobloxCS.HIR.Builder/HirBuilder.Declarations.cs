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
                case IFieldSymbol fieldSymbol: {
                    var field = BuildField(fieldSymbol);
                    fields.Add(field);

                    break;
                }

                // Roslyn synthesizes this when static fields have initializers; those come through HirField instead.
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
        var syntax = Semantics.GetFirstSyntaxFromSymbol<BaseMethodDeclarationSyntax>(method);
        var operation = Semantics.CheckedGetOperation<IMethodBodyBaseOperation>(syntax);

        if ((operation.BlockBody ?? operation.ExpressionBody) is not { } body) {
            throw new InvalidOperationException($"Method '{method.Name}' has no body.");
        }

        return BuildBlock(body);
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
}
