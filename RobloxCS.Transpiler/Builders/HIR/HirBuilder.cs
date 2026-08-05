using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;

namespace RobloxCS.Transpiler.Builders.HIR;

public sealed partial class HirBuilder {
    public TranspilationContext Context { get; }

    public HirBuilder(TranspilationContext ctx) {
        Context = ctx;
    }

    public HirModule Build() {
        var types = Context.Root.DescendantNodes().OfType<TypeDeclarationSyntax>();

        var classes = new List<HirClass>();

        foreach (var type in types) {
            var typeSymbol = Context.Semantics.CheckedGetDeclaredSymbol<INamedTypeSymbol>(type);
            var result = BuildTypeDeclaration(typeSymbol, type);

            classes.Add(result);
        }

        return new HirModule {
            SourcePath = Context.Compiler.FilePath,
            Classes = classes,
        };
    }
}