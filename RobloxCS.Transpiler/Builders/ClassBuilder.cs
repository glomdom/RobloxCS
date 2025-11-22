using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using RobloxCS.Transpiler.Helpers;
using Serilog;

namespace RobloxCS.Transpiler.Builders;

public static class ClassBuilder {
    private static readonly TypeDeclarationStatement NoTypeSentinel = TypeDeclarationStatement.EmptyTable("__SHOULD_NOT_BE_IN_OUTPUT");

    public static IEnumerable<Statement> Build(ClassDeclarationSyntax node, TranspilationContext ctx) {
        var classSymbol = ctx.Semantics.CheckedGetDeclaredSymbol(node);
        var className = classSymbol.Name;

        Log.Information("Transpiling class {ClassName}", className);

        var (instanceDecl, typeDecl, ctorField) = BuildTypeDeclarations(node, ctx);

        CreateClassFields(typeDecl, ctorField, className);

        var bind = new LocalAssignmentStatement {
            Names = [SymbolExpression.FromString(className)],
            Expressions = [TableConstructorExpression.Empty()],
            Types = [],
        };

        var classBody = BuildClassBody(ctx, classSymbol, className);
        var doStmt = DoStatement.FromBlock(classBody);

        return [instanceDecl, typeDecl, bind, doStmt];
    }

    private static (TypeDeclarationStatement instanceDecl, TypeDeclarationStatement typeDecl, TypeField ctorField) BuildTypeDeclarations(
        ClassDeclarationSyntax node,
        TranspilationContext ctx
    ) {
        var className = node.Identifier.ValueText;
        var instanceDecl = TypeDeclarationStatement.EmptyTable($"_Instance{className}");

        {
            var members = node.Members
                .SelectMany(m => GetMemberSymbols(m, ctx))
                .Where(s => s is IFieldSymbol or IPropertySymbol or IMethodSymbol)
                .Where(s => !s.IsStatic)
                .SelectMany(s => TypeFieldBuilder.GenerateTypeFieldsFromField(s, ctx));

            foreach (var tf in members) {
                (instanceDecl.DeclareAs as TableTypeInfo)?.Fields.Add(tf);
            }
        }

        var ctorField = BuildConstructorField(node, instanceDecl.Name, ctx);

        (instanceDecl.DeclareAs as TableTypeInfo)?.Fields.Add(ctorField);

        var typeDecl = TypeDeclarationStatement.EmptyTable($"_Type{className}");

        {
            var members = node.Members
                .SelectMany(m => GetMemberSymbols(m, ctx))
                .Where(s => s is IFieldSymbol or IPropertySymbol or IMethodSymbol)
                .Where(s => s.IsStatic)
                .SelectMany(s => TypeFieldBuilder.GenerateTypeFieldsFromField(s, ctx));

            foreach (var tf in members) {
                (typeDecl.DeclareAs as TableTypeInfo)?.Fields.Add(tf);
            }
        }

        return (instanceDecl, typeDecl, ctorField);
    }

    private static void CreateClassFields(TypeDeclarationStatement typeDecl, TypeField ctorField, string className) {
        if (typeDecl.DeclareAs is not TableTypeInfo typeTable) return;

        var newField = ctorField.DeepClone();
        newField.Key = NameTypeFieldKey.FromString("new");

        if (newField.Value is CallbackTypeInfo { Arguments.Count: > 0 } cb) {
            cb.Arguments.RemoveAt(0); // drop `self`
            cb.ReturnType = BasicTypeInfo.FromString($"_Instance{className}");
        }

        typeTable.Fields.Add(newField);
    }

    private static TypeField BuildConstructorField(ClassDeclarationSyntax node, string instanceTypeName, TranspilationContext ctx) {
        if (ctx.Semantics.GetDeclaredSymbol(node) is not INamedTypeSymbol classSymbol) return DefaultCtor();

        var ctorSymbol = classSymbol.InstanceConstructors.FirstOrDefault(c => !c.IsStatic);
        if (ctorSymbol is null) return DefaultCtor();

        var parameters = ctorSymbol.Parameters
            .Select(p => TypeArgument.From(p.Name, SyntaxUtilities.BasicFromSymbol(p.Type)))
            .ToList();

        var ctorType = new CallbackTypeInfo {
            Arguments = parameters.Prepend(TypeArgument.From("self", BasicTypeInfo.FromString(instanceTypeName))).ToList(),
            ReturnType = BasicTypeInfo.Void(),
        };

        return TypeField.FromNameAndType("constructor", ctorType);

        TypeField DefaultCtor() {
            var cb = new CallbackTypeInfo { Arguments = [], ReturnType = BasicTypeInfo.Void() };
            return TypeField.FromNameAndType("constructor", cb);
        }
    }

    private static Block BuildClassBody(TranspilationContext ctx, INamedTypeSymbol classSymbol, string className) {
        var block = BlockHelpers.Empty();

        var toStringBlock = BlockHelpers.Empty();
        toStringBlock.AddStatement(ReturnStatement.FromExpressions([StringExpression.FromString(className)]));

        var toStringFunction = new AnonymousFunctionExpression {
            Body = new FunctionBody {
                Body = toStringBlock,
                Parameters = [],
                ReturnType = BasicTypeInfo.String(),
                TypeSpecifiers = [],
            },
        };

        block.AddStatement(new AssignmentStatement {
            Vars = [VarName.FromString(className)],
            Expressions = [
                FunctionCallExpression.Basic(
                    "setmetatable",
                    TableConstructorExpression.Empty(),
                    TableConstructorExpression.With(new NameKey { Key = "__tostring", Value = toStringFunction })
                ),
            ],
        });

        block.AddStatement(AssignmentStatement.AssignToSymbol($"{className}.__index", className));

        foreach (var staticField in classSymbol.GetMembers().OfType<IFieldSymbol>().Where(s => s.IsStatic)) {
            var fieldSyntax = SyntaxUtilities.GetSyntaxFromSymbol<VariableDeclaratorSyntax>(staticField);

            var initializer = fieldSyntax.Initializer?.Value;
            if (initializer is null) {
                throw new NotSupportedException("Fields without initializers are not yet supported.");
            }

            var exprResult = ExpressionBuilder.BuildFromSyntax(initializer, ctx);
            var staticAssignment = AssignmentStatement.AssignTo($"{staticField.ContainingSymbol.Name}.{staticField.Name}", exprResult.Expression);

            block.AddStatement(staticAssignment);
        }

        foreach (var ctor in classSymbol.InstanceConstructors) {
            block.AddStatement(FunctionBuilder.CreateNewMethod(classSymbol, ctor));
        }

        foreach (var ctor in classSymbol.InstanceConstructors) {
            block.AddStatement(FunctionBuilder.CreateConstructor(classSymbol, ctor, ctx));
        }

        foreach (var method in classSymbol.GetMembers().OfType<IMethodSymbol>()) {
            if (method.MethodKind == MethodKind.Constructor) continue;
            if (method.MethodKind == MethodKind.StaticConstructor) continue; // TODO: use this instead of generating static fields

            var methodStmt = FunctionBuilder.BuildFromMethodSymbol(method, ctx);
            block.AddStatement(methodStmt);
        }

        return block;
    }

    private static IEnumerable<ISymbol> GetMemberSymbols(
        MemberDeclarationSyntax m,
        TranspilationContext ctx
    ) {
        if (m is FieldDeclarationSyntax f) {
            foreach (var sym in f.Declaration.Variables.Select(v => ctx.Semantics.GetDeclaredSymbol(v)).OfType<ISymbol>()) {
                yield return sym;
            }

            yield break;
        }

        var s = ctx.Semantics.GetDeclaredSymbol(m);
        if (s is not null) yield return s;
    }

}