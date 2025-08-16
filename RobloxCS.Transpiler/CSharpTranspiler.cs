using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using RobloxCS.Transpiler.Scoping;

namespace RobloxCS.Transpiler;

/// <summary>
/// The big momma of RobloxCS
/// </summary>
public sealed class CSharpTranspiler : CSharpSyntaxWalker {
    public TranspilerOptions Options { get; }
    public CSharpCompiler Compiler { get; }
    public SemanticModel Semantics { get; }
    public CompilationUnitSyntax Root { get; }

    public List<AstNode> Nodes = [];
    public List<Statement> Exports = [];

    public Block? CurrentBlock { get; set; }
    public TypeDeclaration? CurrentTypeDeclaration { get; set; }

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Options = options;
        Compiler = compiler;

        Root = Compiler.Root;
        Semantics = Compiler.Compilation.GetSemanticModel(Root.SyntaxTree);
    }

    public void Transpile() {
        Visit(Root);

        if (Options.ScriptType != ScriptType.Module) return;

        // handle module exports (functions, etc)
        var moduleReturn = Exports.Count == 0 ? Return.FromExpressions([SymbolExpression.FromString("nil")]) : Return.Empty();
        Nodes.Add(moduleReturn);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node) {
        var (instanceDecl, typeDecl, ctorFieldMaybe) = BuildClassTypeDeclarations(node);

        Block classBlock;
        using (WithBlock(Block.Empty())) {
            VisitMembers(node.Members, this);
            classBlock = CurrentBlock!;
        }

        if (ctorFieldMaybe is { } ctorField && typeDecl.DeclareAs is TableTypeInfo typeTable) {
            ctorField.Key = NameTypeFieldKey.FromString("new");
            if (ctorField.Value is CallbackTypeInfo { Arguments.Count: > 0 } cb) {
                cb.Arguments.RemoveAt(0); // remove self
            }

            typeTable.Fields.Add(ctorField);

            var index = TypeField.FromNameAndType("__index", BasicTypeInfo.FromString(typeDecl.Name));
            typeTable.Fields.Add(index);
        }

        Nodes.Add(instanceDecl);
        Nodes.Add(typeDecl);

        var both = IntersectionTypeInfo.FromNames(instanceDecl.Name, typeDecl.Name);
        Nodes.Add(LocalAssignment.Naked(node.Identifier.ValueText, both));
        Nodes.Add(DoStatement.FromBlock(classBlock));
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node) {
        if (WithTableFields(table => {
            foreach (var f in GenerateTypeFieldsFromField(node)) table.Fields.Add(f);
        })) {
            if (CurrentBlock is null) return;
        } else {
            if (CurrentBlock is null) return;
        }

        foreach (var decl in node.Declaration.Variables) {
            Console.WriteLine($"TODO: implement declaration for {decl.Identifier.ValueText}");
        }
    }

    private (TypeDeclaration instanceDecl, TypeDeclaration typeDecl, TypeField? ctorField) BuildClassTypeDeclarations(ClassDeclarationSyntax node) {
        var className = node.Identifier.ValueText;

        var instanceDecl = TypeDeclaration.EmptyTable($"_Instance{className}");
        using (WithType(instanceDecl)) {
            VisitMembers(node.Members, this);
        }

        var ctorField = TryBuildConstructorField(node, instanceDecl.Name);

        var typeDecl = TypeDeclaration.EmptyTable($"_Type{className}");
        using (WithType(typeDecl)) {
            // todo: visit statics
        }

        return (instanceDecl, typeDecl, ctorField);
    }

    private TypeField? TryBuildConstructorField(ClassDeclarationSyntax node, string instanceTypeName) {
        var classSymbol = Semantics.GetDeclaredSymbol(node);
        if (classSymbol is null) return DefaultCtor();

        var ctorSymbol = classSymbol.Constructors.FirstOrDefault(c => !c.IsStatic);
        if (ctorSymbol is null) return null;

        var parameters = ctorSymbol.Parameters.Select(p =>
            TypeArgument.From(p.Name, SyntaxUtilities.BasicFromSymbol(p.Type))
        ).ToList();

        var ctorType = new CallbackTypeInfo {
            Arguments = parameters.Prepend(TypeArgument.From("self", BasicTypeInfo.FromString(instanceTypeName))).ToList(),
            ReturnType = BasicTypeInfo.Void()
        };

        return TypeField.FromNameAndType("constructor", ctorType);

        TypeField DefaultCtor() {
            var cb = new CallbackTypeInfo {
                Arguments = [],
                ReturnType = BasicTypeInfo.Void()
            };

            return TypeField.FromNameAndType("constructor", cb);
        }
    }

    private IEnumerable<TypeField> GenerateTypeFieldsFromField(FieldDeclarationSyntax fieldSyntax) {
        var decl = fieldSyntax.Declaration;
        var fieldType = InferNonnull(decl.Type);
        var primitiveType = BasicTypeInfo.FromString(SyntaxUtilities.MapPrimitive(fieldType));

        var isReadonly = fieldSyntax.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);

        foreach (var v in decl.Variables) {
            yield return new TypeField {
                Key = NameTypeFieldKey.FromString(v.Identifier.ValueText),
                Access = isReadonly ? AccessModifier.Read : null,
                Value = primitiveType
            };
        }
    }

    private void ProcessClassRuntimeFields(ClassDeclarationSyntax node) {
        var classBlock = Block.Empty();
        CurrentBlock = classBlock;

        foreach (var member in node.Members) {
            Visit(member);
        }

        CurrentBlock = null;
        Nodes.Add(DoStatement.FromBlock(classBlock));
    }

    private ITypeSymbol InferNonnull(TypeSyntax syntax) {
        var fieldType = Semantics.GetTypeInfo(syntax).Type!;

        return fieldType is IErrorTypeSymbol or null ? throw new Exception("Error occured while attempting to infer type.") : fieldType;
    }

    private bool WithTableFields(Action<TableTypeInfo> action) {
        if (CurrentTypeDeclaration?.DeclareAs is not TableTypeInfo table) return false;

        action(table);

        return true;
    }

    private IDisposable WithBlock(Block block) {
        var prev = CurrentBlock;
        CurrentBlock = block;

        return new Scope<Block?>(v => CurrentBlock = v, prev);
    }

    private IDisposable WithType(TypeDeclaration decl) {
        var prev = CurrentTypeDeclaration;
        CurrentTypeDeclaration = decl;

        return new Scope<TypeDeclaration?>(v => CurrentTypeDeclaration = v, prev);
    }

    private static void VisitMembers(SyntaxList<MemberDeclarationSyntax> members, CSharpSyntaxWalker walker) {
        foreach (var member in members) {
            walker.Visit(member);
        }
    }
}