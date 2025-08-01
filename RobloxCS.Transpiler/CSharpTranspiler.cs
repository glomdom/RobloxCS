using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler;

/// <summary>
/// The big momma of RobloxCS
/// </summary>
public sealed class CSharpTranspiler : CSharpSyntaxWalker {
    public TranspilerOptions Options { get; }
    public CSharpCompiler Compiler { get; }
    public SemanticModel Semantics => Compiler.Compilation.GetSemanticModel(Root.SyntaxTree);
    public CompilationUnitSyntax Root => Compiler.Root;

    public List<AstNode> Nodes { get; set; } = [];
    public List<Statement> Exports { get; set; } = [];

    public Block? CurrentBlock { get; set; }

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Options = options;
        Compiler = compiler;
    }

    public void Transpile() {
        Visit(Root);
        
        if (Options.ScriptType != ScriptType.Module) return;

        // handle module exports (functions, etc)
        var moduleReturn = Exports.Count == 0 ? new Return([new SymbolExpression("nil")]) : new Return([]);
        Nodes.Add(moduleReturn);
    }
    
    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        var className = node.Identifier.ValueText;
    
        var local = LocalAssignment.AloneWithoutExpr(className);
        Nodes.Add(local);
    
        var classBlock = Block.Empty();
        classBlock.AddStatement(new Assignment([VarName.FromString(className)], [new BooleanExpression(false)]));
        
        CurrentBlock = classBlock;
        foreach (var member in node.Members) {
            Visit(member);
        }
    
        CurrentBlock = null;
        Nodes.Add(DoStatement.From(classBlock));
    }
    
    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        if (CurrentBlock is null) {
            throw new Exception("Attempted to visit field declaration when CurrentBlock is null.");
        }
    
        foreach (var decl in node.Declaration.Variables) {
            Console.WriteLine($"TODO: implement declaration for {decl.Identifier.ValueText}");
        }
    
        base.VisitFieldDeclaration(node);
    }
}