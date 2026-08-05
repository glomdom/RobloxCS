using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using Serilog;

namespace RobloxCS.Transpiler.Builders.HIR;

public sealed partial class HirBuilder {
    public HirClass BuildTypeDeclaration(INamedTypeSymbol typeSymbol, TypeDeclarationSyntax typeSyntax) {
        var methods = new List<HirMethod>();
        var fields = new List<HirField>();

        foreach (var member in typeSymbol.GetMembers()) {
            if (member is IFieldSymbol fieldSymbol) {
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
            }

            // if (member is IMethodSymbol { MethodKind: MethodKind.Ordinary } method) {
            //     methods.Add(new HirMethod {
            //         Location = SyntaxUtilities.ResolveLocations(method.Locations),
            //         Symbol = method,
            //         
            //     });
            // }
        }

        return new HirClass {
            Location = SyntaxUtilities.ResolveLocations(typeSymbol.Locations),
            Symbol = typeSymbol,
            Base = typeSymbol.BaseType,
            Fields = fields,
            Methods = methods,
            Properties = [],
        };
    }
}