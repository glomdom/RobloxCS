using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;
using Serilog;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Transpiler.Builders;

internal static class ClassBuilder {
    private static readonly TypeDeclaration NoTypeSentinel = TypeDeclaration.EmptyTable("__SHOULD_NOT_BE_IN_OUTPUT");

    public static IEnumerable<Statement> Build(ClassDeclarationSyntax node, TranspilationContext ctx) {
        var classSymbol = ctx.Semantics.CheckedGetDeclaredSymbol(node);
        var className = classSymbol.Name;

        Log.Information("Transpiling class {ClassName}", className);

        var (instanceDecl, typeDecl, ctorField) = BuildTypeDeclarations(node, ctx);

        InjectTypeSugar(typeDecl, ctorField, className);

        var both = IntersectionTypeInfo.FromNames(instanceDecl.Name, typeDecl.Name);
        var bind = LocalAssignment.Naked(className, both);
        var classBody = BuildClassBody(ctx, classSymbol, className);
        var doStmt = DoStatement.FromBlock(classBody);

        return [instanceDecl, typeDecl, bind, doStmt];
    }

    private static (TypeDeclaration instanceDecl, TypeDeclaration typeDecl, TypeField ctorField) BuildTypeDeclarations(
        ClassDeclarationSyntax node,
        TranspilationContext ctx
    ) {
        var className = node.Identifier.ValueText;
        var instanceDecl = TypeDeclaration.EmptyTable($"_Instance{className}");

        {
            foreach (var member in node.Members) {
                if (member is not FieldDeclarationSyntax f) continue;

                foreach (var tf in FieldBuilder.GenerateTypeFieldsFromField(f, ctx)) {
                    (instanceDecl.DeclareAs as TableTypeInfo)?.Fields.Add(tf);
                }

                // TODO: instance methods -> type fields or method tables (future)
            }
        }

        var ctorField = BuildConstructorField(node, instanceDecl.Name, ctx);

        (instanceDecl.DeclareAs as TableTypeInfo)?.Fields.Add(ctorField);

        var typeDecl = TypeDeclaration.EmptyTable($"_Type{className}");
        // TODO: visit statics and add to typeDecl

        return (instanceDecl, typeDecl, ctorField);
    }

    private static void InjectTypeSugar(TypeDeclaration typeDecl, TypeField ctorField, string className) {
        if (typeDecl.DeclareAs is not TableTypeInfo typeTable) return;

        var newField = ctorField.DeepClone();
        newField.Key = NameTypeFieldKey.FromString("new");

        if (newField.Value is CallbackTypeInfo { Arguments.Count: > 0 } cb) {
            cb.Arguments.RemoveAt(0); // drop `self`
            cb.ReturnType = BasicTypeInfo.FromString($"_Instance{className}");
        }

        typeTable.Fields.Add(newField);

        var index = TypeField.FromNameAndType("__index", BasicTypeInfo.FromString(typeDecl.Name));
        typeTable.Fields.Add(index);

        var tostring = TypeField.FromNameAndType("__tostring", new CallbackTypeInfo { Arguments = [], ReturnType = BasicTypeInfo.String() });

        typeTable.Fields.Add(tostring);
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
        var block = Block.Empty();

        var toStringBlock = Block.Empty();
        toStringBlock.AddStatement(Return.FromExpressions([StringExpression.FromString(className)]));

        var toStringFunction = new AnonymousFunction {
            Body = new FunctionBody {
                Body = toStringBlock,
                Parameters = [],
                ReturnType = BasicTypeInfo.String(),
                TypeSpecifiers = [],
            },
        };

        block.AddStatement(new Assignment {
            Vars = [VarName.FromString(className)],
            Expressions = [
                FunctionCall.Basic(
                    "setmetatable",
                    TableConstructor.Empty(),
                    TableConstructor.With(new NameKey { Key = "__tostring", Value = toStringFunction })
                ),
            ],
        });

        block.AddStatement(Assignment.AssignToSymbol($"{className}.__index", className));

        foreach (var ctor in classSymbol.InstanceConstructors) {
            block.AddStatement(FunctionBuilder.CreateNewMethod(classSymbol, ctor));
        }

        foreach (var ctor in classSymbol.InstanceConstructors) {
            block.AddStatement(FunctionBuilder.CreateConstructor(classSymbol, ctor, ctx));
        }

        return block;
    }
}