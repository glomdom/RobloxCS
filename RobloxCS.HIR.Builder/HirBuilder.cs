using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.Common.Diagnostics;
using RobloxCS.Compiler;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR.Builder;

/// <summary>
/// Builds a <see cref="HirModule"/> from a single source file using Roslyn's IOperation tree.
/// </summary>
public sealed partial class HirBuilder {
    public CSharpCompiler Compiler { get; }
    public SemanticModel Semantics { get; }
    public CompilationUnitSyntax Root { get; }
    public DiagnosticBag Diagnostics { get; }

    public HirBuilder(CSharpCompiler compiler, DiagnosticBag diagnostics) {
        Compiler = compiler;
        Diagnostics = diagnostics;
        Root = compiler.Root;
        Semantics = compiler.Compilation.GetSemanticModel(Root.SyntaxTree);
    }

    public HirModule Build() {
        var types = Root.DescendantNodes().OfType<TypeDeclarationSyntax>();
        var classes = new List<HirType>();

        foreach (var type in types) {
            var typeSymbol = Semantics.CheckedGetDeclaredSymbol<INamedTypeSymbol>(type);
            var result = BuildType(typeSymbol, type);

            classes.Add(result);
        }

        return new HirModule {
            Location = Location.None,
            SourcePath = Compiler.FilePath,
            Types = [.. classes],
        };
    }

    /// <summary>
    /// Reports unsupported syntax and returns an error node so building can continue.
    /// </summary>
    private HirErrorStatement Unsupported(string message, Location location) {
        Diagnostics.Error(DiagnosticId.UnsupportedSyntax, message, location);

        return new HirErrorStatement {
            Location = location,
        };
    }
}
