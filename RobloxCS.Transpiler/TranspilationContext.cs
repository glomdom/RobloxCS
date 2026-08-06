using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.Compiler;

namespace RobloxCS.Transpiler;

public sealed class TranspilationContext {
    public TranspilerOptions Options { get; }
    public CSharpCompiler Compiler { get; }
    public SemanticModel Semantics { get; }
    public CompilationUnitSyntax Root { get; }

    public TranspilationContext(TranspilerOptions options, CSharpCompiler compiler) {
        Options = options;
        Compiler = compiler;
        Root = compiler.Root;
        Semantics = compiler.Compilation.GetSemanticModel(Root.SyntaxTree);
    }
}