using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;

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
        var moduleReturn = Exports.Count == 0 ? Return.FromExpressions([SymbolExpression.FromString("nil")]) : Return.Empty();
        Nodes.Add(moduleReturn);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        ProcessClassTypeDeclaration(node);
        ProcessClassRuntimeFields(node);
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

        var fieldType = InferNonnull(decl.Type);
        var primitiveType = BasicTypeInfo.FromString(SyntaxUtilities.MapPrimitive(fieldType));

        if (variables.Count == 1) {
            var var = variables[0];
            var isReadonly = fieldSyntax.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);

            return [
                new TypeField {
                    Key = NameTypeFieldKey.FromString(var.Identifier.ValueText),
                    Access = isReadonly ? AccessModifier.Read : null,
                    Value = primitiveType,
                }
            ];
        } else {
            var varNames = variables.Select(v => v.Identifier.ValueText);
            var isReadonly = fieldSyntax.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);

            var types = varNames.Select(name => new TypeField {
                Key = NameTypeFieldKey.FromString(name),
                Access = isReadonly ? AccessModifier.Read : null,
                Value = primitiveType
            });

            return [..types];
        }
    }

    private void ProcessClassTypeDeclaration(ClassDeclarationSyntax node) {
        var className = node.Identifier.ValueText;

        TypeField? ctorDecl = null;
        var classInstanceDecl = TypeDeclaration.EmptyTable($"_Instance{className}");
        CurrentTypeDeclaration = classInstanceDecl;

        foreach (var member in node.Members) {
            Visit(member);
        }

        var classSymbol = Semantics.GetDeclaredSymbol(node);
        if (classSymbol != null) {
            var ctors = classSymbol.Constructors
                .Where(c => !c.IsStatic)
                .ToList();

            if (ctors.Count > 0) {
                foreach (var ctorSymbol in ctors) {
                    var parameters = ctorSymbol.Parameters.Select(p =>
                        TypeArgument.From(
                            p.Name,
                            SyntaxUtilities.BasicFromSymbol(p.Type)
                        )
                    ).ToList();

                    var ctorType = new CallbackTypeInfo {
                        Arguments =
                            parameters.Prepend(TypeArgument.From("self", BasicTypeInfo.FromString(classInstanceDecl.Name))).ToList(),
                        ReturnType = BasicTypeInfo.Void()
                    };

                    if (CurrentTypeDeclaration!.DeclareAs is TableTypeInfo tableInfo) {
                        ctorDecl = TypeField.FromNameAndType("constructor", ctorType);
                        tableInfo.Fields.Add(ctorDecl.DeepClone());
                    }

                    break; // TODO: more than 1 ctor
                }
            } else {
                var ctorType = new CallbackTypeInfo {
                    Arguments = [],
                    ReturnType = BasicTypeInfo.Void()
                };

                if (CurrentTypeDeclaration!.DeclareAs is TableTypeInfo tableInfo) {
                    tableInfo.Fields.Add(TypeField.FromNameAndType("constructor", ctorType));
                }
            }
        }

        CurrentTypeDeclaration = null;
        Nodes.Add(classInstanceDecl);

        var classTypeTypeDecl = TypeDeclaration.EmptyTable($"_Type{className}");
        CurrentTypeDeclaration = classTypeTypeDecl;

        if (CurrentTypeDeclaration!.DeclareAs is TableTypeInfo table) {
            ctorDecl!.Key = NameTypeFieldKey.FromString("new");
            (ctorDecl.Value as CallbackTypeInfo)!.Arguments.RemoveAt(0); // remove self param
            table.Fields.Add(ctorDecl);

            var index = TypeField.FromNameAndType("__index", BasicTypeInfo.FromString(classTypeTypeDecl.Name));
            table.Fields.Add(index);
        }

        CurrentTypeDeclaration = null;
        Nodes.Add(classTypeTypeDecl);

        var local = LocalAssignment.Naked(className, IntersectionTypeInfo.FromNames(classInstanceDecl.Name, classTypeTypeDecl.Name));
        Nodes.Add(local);
    }

    private void ProcessClassRuntimeFields(ClassDeclarationSyntax node) {
        var className = node.Identifier.ValueText;

        var classBlock = Block.Empty();
        CurrentBlock = classBlock;

        foreach (var member in node.Members) {
            Visit(member);
        }

        CurrentBlock = null;
        Nodes.Add(DoStatement.From(classBlock));
    }

    private ITypeSymbol InferNonnull(TypeSyntax syntax) {
        var fieldType = Semantics.GetTypeInfo(syntax).Type!;
        if (fieldType is IErrorTypeSymbol) {
            throw new Exception("Error occured while attempting to infer type.");
        }

        return fieldType;
    }
}