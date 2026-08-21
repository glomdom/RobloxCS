using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;

namespace RobloxCS.Transpiler.Builders.HIR;

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

                case IMethodSymbol methodSymbol: {
                    var method = BuildMethod(methodSymbol);
                    methods.Add(method);

                    break;
                }
            }
        }

        return new HirType {
            Location = SyntaxUtilities.ResolveLocations(typeSymbol.Locations),
            Symbol = typeSymbol,
            Base = typeSymbol.BaseType,
            Fields = fields,
            Methods = methods,
            Properties = [],
        };
    }

    public HirField BuildField(IFieldSymbol field) {
        if (field.IsImplicitlyDeclared) {
            Log.Verbose("Skipping {FieldName} as it is implicitly declared", field.Name);
        }

        var syntax = Context.Semantics.GetFirstSyntaxFromSymbol<VariableDeclaratorSyntax>(field);
        var hasInitializer = syntax.Initializer is not null;

        HirExpression? initializer = null;
        if (hasInitializer) {
            var operation = Context.Semantics.CheckedGetOperation<IFieldInitializerOperation>(syntax.Initializer!);

            initializer = BuildExpression(operation.Value);
        }

        return new HirField {
            Location = SyntaxUtilities.ResolveLocations(field.Locations),
            Symbol = field,
            Initializer = initializer,
            IsStatic = field.IsStatic,
        };
    }

    public HirMethod BuildMethod(IMethodSymbol method) {
        Log.Verbose("Adding method {MethodName} of kind {MethodKind}", method.Name, method.MethodKind);
        
        var entryPointAttr = Context.Compiler.Compilation.GetTypeByMetadataName("RobloxCS.Types.Attributes.EntryPointAttribute");
        if (entryPointAttr is null) {
            throw new InvalidOperationException("Failed to get 'EntryPointAttribute' from compilation.");
        }

        var isCtor = method is { MethodKind: MethodKind.Constructor };
        var isImplicitCtor = method is { IsImplicitlyDeclared: true } && isCtor;

        var isEntryPoint = false;
        foreach (var attrData in method.GetAttributes()) {
            if (SymbolEqualityComparer.Default.Equals(attrData.AttributeClass, entryPointAttr)) {
                isEntryPoint = true;
            }
        }

        var parameters = method.Parameters.Select(BuildParameter).ToList();
        var statements = new List<HirStatement>();

        var block = new HirBlock { Location = Location.None, Statements = [], Locals = [] };
        if (!isImplicitCtor) {
            var syntax = Context.Semantics.GetFirstSyntaxFromSymbol<BaseMethodDeclarationSyntax>(method);
            var operation = Context.Semantics.CheckedGetOperation<IMethodBodyOperation>(syntax);
 
            if (operation.BlockBody is { } body) {
                statements.AddRange(body.Operations.Select(BuildStatement));

                block = block with { Statements = [.. statements], Locals = [.. body.Locals] };
            } else {
                var stmt = Context.Diagnostics.UnsupportedStatement("expression bodies are not supported", SyntaxUtilities.ResolveLocations(method.Locations));

                statements.Add(stmt);
            }
        } else {
            Log.Debug("Reconstructing implicit constructor {MethodName}", method.Name);
            Log.Warning("Locals are not yet reconstructed for implicit constructors");

            if (method.DeclaringSyntaxReferences.FirstOrDefault() is not null) {
                throw new InvalidOperationException("Implicit constructor contains a declaring syntax.");
            }

            var container = method.ContainingSymbol;
            if (container is not INamedTypeSymbol cls) {
                throw new NotSupportedException("Reconstructing implicit constructor is not supported for types other than classes.");
            }

            foreach (var classMember in cls.GetMembers()) {
                if (classMember is not IFieldSymbol fieldSymbol) continue;

                if (fieldSymbol.AssociatedSymbol is not null) {
                    throw new NotSupportedException("Backing fields are not yet supported.");
                }

                var syntax = Context.Semantics.GetFirstSyntaxFromSymbol<VariableDeclaratorSyntax>(fieldSymbol);
                if (syntax.Initializer is null) {
                    throw new NotSupportedException("No initializer fields are not yet supported.");
                }

                Log.Verbose("Added field {FieldSymbolName} to synthesized ctor", fieldSymbol.Name);

                var fieldOperation = Context.Semantics.CheckedGetOperation<IFieldInitializerOperation>(syntax.Initializer);
                statements.Add(BuildStatement(fieldOperation));
            }

            block = block with { Statements = [.. statements], Locals = [] };
        }

        return new HirMethod {
            Location = SyntaxUtilities.ResolveLocations(method.Locations),
            Symbol = method,
            Parameters = [.. parameters],
            TypeParameters = [], // todo
            Block = block,
            IsStatic = method.IsStatic,
            IsConstructor = isCtor,
            IsEntryPoint = isEntryPoint,
            IsImplicit = method is { IsImplicitlyDeclared: true }
        };
    }
}