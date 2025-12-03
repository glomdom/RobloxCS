using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.Compiler;
using RobloxCS.Transpiler.Helpers;
using RobloxCS.Transpiler.Scoping;

namespace RobloxCS.Transpiler;

public sealed class TranspilationContext {
    public TranspilerOptions Options { get; }
    public CSharpCompiler Compiler { get; }
    public SemanticModel Semantics { get; }
    public CompilationUnitSyntax Root { get; }
    public Block RootBlock { get; set; } = BlockHelpers.Empty();

    public TranspilationContext(TranspilerOptions options, CSharpCompiler compiler) {
        Options = options;
        Compiler = compiler;
        Root = compiler.Root;
        Semantics = compiler.Compilation.GetSemanticModel(Root.SyntaxTree);
    }

    public Chunk ToChunk() {
        var chunk = new Chunk { Block = RootBlock };

        return chunk;
    }

    public void Add(params Statement[] statements) {
        foreach (var s in statements) RootBlock.AddStatement(s);
    }
}