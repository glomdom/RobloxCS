using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

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
    public TypeDeclaration? CurrentTypeDeclaration { get; set; }

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

        var classInstanceDecl = TypeDeclaration.EmptyTable($"_Instance{className}");
        CurrentTypeDeclaration = classInstanceDecl;

        foreach (var member in node.Members) {
            Visit(member);
        }

        CurrentTypeDeclaration = null;
        Nodes.Add(classInstanceDecl);

        var local = LocalAssignment.Naked(className);
        Nodes.Add(local);

        var classBlock = Block.Empty();
        classBlock.AddStatement(new Assignment([VarName.FromString(className), VarName.FromString("doubleTest")],
            [new BooleanExpression(false), new BooleanExpression(true)]));

        CurrentBlock = classBlock;
        foreach (var member in node.Members) {
            Visit(member);
        }

        CurrentBlock = null;
        Nodes.Add(DoStatement.From(classBlock));
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        if (CurrentTypeDeclaration?.DeclareAs is TableTypeInfo tableInfo) {
            var typeFields = GenerateTypeFieldsFromField(node);
            foreach (var field in typeFields) {
                tableInfo.Fields.Add(field);
            }

            return;
        }

        if (CurrentBlock is null) {
            throw new Exception("Attempted to visit field declaration when CurrentBlock is null.");
        }

        foreach (var decl in node.Declaration.Variables) {
            Console.WriteLine($"TODO: implement declaration for {decl.Identifier.ValueText}");
        }

        base.VisitFieldDeclaration(node);
    }

    private List<TypeField> GenerateTypeFieldsFromField(FieldDeclarationSyntax fieldSyntax) {
        var decl = fieldSyntax.Declaration;
        var variables = decl.Variables;

        if (variables.Count == 1) {
            var var = variables[0];
            var fieldType = Semantics.GetTypeInfo(decl.Type).Type!; // this shouldn't error

            return [
                new TypeField {
                    Key = NameTypeFieldKey.FromString(var.Identifier.ValueText),
                    Value = BasicTypeInfo.FromString(SyntaxUtilities.MapPrimitive(fieldType))
                }
            ];
        } else {
            var varNames = variables.Select(v => v.Identifier.ValueText);
            var fieldType = Semantics.GetTypeInfo(decl.Type).Type!;
            var primitiveType = BasicTypeInfo.FromString(SyntaxUtilities.MapPrimitive(fieldType));

            var types = varNames.Select(name => new TypeField { Key = NameTypeFieldKey.FromString(name), Value = primitiveType });

            return [..types];
        }
    }
}