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
                    if (fieldSymbol.IsImplicitlyDeclared) {
                        Log.Verbose("Skipping {FieldName} as it is implicitly declared", fieldSymbol.Name);
                    }

                    var syntax = Context.Semantics.GetFirstSyntaxFromSymbol<VariableDeclaratorSyntax>(member);
                    var hasInitializer = syntax.Initializer is not null;

                    HirExpression? initializer = null;
                    if (hasInitializer) {
                        var operation = Context.Semantics.CheckedGetOperation<IFieldInitializerOperation>(syntax.Initializer!);

                        initializer = BuildExpression(operation.Value);
                    }

                    var field = new HirField {
                        Location = SyntaxUtilities.ResolveLocations(fieldSymbol.Locations),
                        Symbol = fieldSymbol,
                        Initializer = initializer,
                        IsStatic = fieldSymbol.IsStatic,
                    };

                    fields.Add(field);

                    break;
                }

                case IMethodSymbol method: {
                    Log.Verbose("Adding method {MethodName} of kind {MethodKind}", method.Name, method.MethodKind);

                    var isCtor = method is { MethodKind: MethodKind.Constructor };
                    var isImplicitCtor = method is { IsImplicitlyDeclared: true } && isCtor;

                    var parameters = method.Parameters.Select(BuildParameter).ToList();

                    var block = new HirBlock { Location = Location.None, Statements = [], Locals = [] };
                    if (!isImplicitCtor) {
                        var syntax = Context.Semantics.GetFirstSyntaxFromSymbol<BaseMethodDeclarationSyntax>(method);
                        var operation = Context.Semantics.CheckedGetOperation<IMethodBodyOperation>(syntax);
                        var statements = new List<HirStatement>();

                        if (operation.BlockBody is { } body) {
                            foreach (var bodyOperation in body.Operations) {
                                var stmt = BuildStatement(bodyOperation);

                                statements.Add(stmt);
                            }

                            block = block with { Statements = statements, Locals = [.. body.Locals] };
                        } else {
                            throw new NotSupportedException("Expression bodies are not supported yet.");
                        }
                    }

                    methods.Add(new HirMethod {
                        Location = SyntaxUtilities.ResolveLocations(method.Locations),
                        Symbol = method,
                        Parameters = parameters,
                        TypeParameters = [], // todo
                        Block = block,
                        IsStatic = method.IsStatic,
                        IsConstructor = isCtor,
                        IsEntryPoint = false,
                    });

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
}