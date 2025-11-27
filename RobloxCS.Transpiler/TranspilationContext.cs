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
    public Block RootBlock { get; } = BlockHelpers.Empty();

    public Scope CurrentScope => _scopes.Peek();

    private readonly Stack<Scope> _scopes = new();

    public TranspilationContext(TranspilerOptions options, CSharpCompiler compiler) {
        Options = options;
        Compiler = compiler;
        Root = compiler.Root;
        Semantics = compiler.Compilation.GetSemanticModel(Root.SyntaxTree);
    }

    public void PushScope() {
        var parent = _scopes.Count > 0 ? _scopes.Peek() : null;

        _scopes.Push(new Scope(parent));
    }

    public void PopScope() => _scopes.Pop();

    public Chunk ToChunk() {
        var chunk = new Chunk { Block = RootBlock };

        return chunk;
    }

    public void Add(params Statement[] statements) {
        foreach (var s in statements) RootBlock.AddStatement(s);
    }
}