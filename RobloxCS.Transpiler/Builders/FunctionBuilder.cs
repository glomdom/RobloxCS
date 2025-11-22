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
using RobloxCS.Transpiler.Helpers;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Transpiler.Builders;

public static class FunctionBuilder {
    public static FunctionDeclarationStatement CreateNewMethod(INamedTypeSymbol classSymbol, IMethodSymbol ctorSymbol) {
        var paramNames = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => p.Name).ToList();

        var args = paramNames.Select(NameParameter.FromString).Cast<Parameter>().ToList();
        var specs = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => SyntaxUtilities.BasicFromSymbol(p.Type)).Cast<TypeInfo>().ToList();

        var body = CreateNewMethodBlock(classSymbol, new FunctionArgs {
            Arguments = paramNames.Select(SymbolExpression.FromString).Cast<Expression>().ToList(),
        });

        return new FunctionDeclarationStatement {
            Name = FunctionName.FromString($"{classSymbol.Name}.new"),
            Body = new FunctionBody {
                Parameters = args,
                TypeSpecifiers = specs,
                Body = body,
                ReturnType = BasicTypeInfo.FromString($"_Instance{classSymbol.Name}"),
            },
        };
    }

    private static Block CreateNewMethodBlock(INamedTypeSymbol classSymbol, FunctionArgs ctorArgs) {
        var block = BlockHelpers.Empty();

        // local self = setmetatable({}, Class)
        block.AddStatement(new LocalAssignmentStatement {
            Names = [SymbolExpression.FromString("self")],
            Expressions = [
                FunctionCallExpression.Basic(
                    "setmetatable",
                    TableConstructorExpression.Empty(),
                    SymbolExpression.FromString(classSymbol.Name)
                ),
            ],
            Types = [],
        });

        // self:constructor(...)
        block.AddStatement(new FunctionCallStatement {
            Prefix = NamePrefix.FromString("self"),
            Suffixes = [new MethodCall { Name = "constructor", Args = ctorArgs }],
        });

        // return self
        block.AddStatement(new ReturnStatement { Returns = [SymbolExpression.FromString("self")] });

        return block;
    }

    public static FunctionDeclarationStatement CreateConstructor(INamedTypeSymbol classSymbol, IMethodSymbol ctorSymbol, TranspilationContext ctx) {
        var functionBlock = BlockHelpers.Empty();

        var fields = classSymbol.GetMembers().OfType<IFieldSymbol>();
        foreach (var a in TypeFieldBuilder.CreateFieldAssignmentsFromFields(fields, ctx)) {
            functionBlock.AddStatement(a);
        }

        var ctorStmt = new FunctionDeclarationStatement {
            Name = FunctionName.FromString($"{classSymbol.Name}:constructor"),
            Body = new FunctionBody {
                Body = functionBlock,
                Parameters = [],
                TypeSpecifiers = [],
                ReturnType = BasicTypeInfo.Void(),
            },
        };

        // populate function block
        var ctorSyntax = SyntaxUtilities.MaybeGetSyntaxFromSymbol<ConstructorDeclarationSyntax>(ctorSymbol);
        if (ctorSyntax is null) {
            // ctor doesnt exist, return just initializers
            return ctorStmt;
        }

        if (ctorSyntax.Body is { } body) {
            ctx.PushScope();

            foreach (var transpiled in body.Statements.Select(stmt => StatementBuilder.Build(stmt, ctx))) {
                functionBlock.AddStatements(transpiled.Statements);
            }

            ctx.PopScope();
        }

        var pars = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => NameParameter.FromString(p.Name)).Cast<Parameter>().ToList();

        ctorStmt.Body.Parameters = pars;

        var specs = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => SyntaxUtilities.BasicFromSymbol(p.Type)).Cast<TypeInfo>().ToList();

        ctorStmt.Body.TypeSpecifiers = specs;

        return ctorStmt;
    }

    public static Statement BuildFromMethodSymbol(IMethodSymbol symbol, TranspilationContext ctx) {
        var pars = symbol.Parameters.Select(p => NameParameter.FromString(p.Name)).Cast<Parameter>().ToList();
        var specs = symbol.Parameters.Select(p => SyntaxUtilities.BasicFromSymbol(p.Type)).Cast<TypeInfo>().ToList();
        var returnType = SyntaxUtilities.BasicFromSymbol(symbol.ReturnType);

        var cls = symbol.ContainingSymbol;
        if (cls is null) {
            throw new Exception("Could not find containing class symbol.");
        }

        var functionBlock = BlockHelpers.Empty();

        var selfType = BasicTypeInfo.FromString($"_Instance{cls.Name}");
        var selfShadowAssignment = LocalAssignmentStatement.Single("self", SymbolExpression.FromString("self"), selfType);
        functionBlock.AddStatement(selfShadowAssignment);

        var syntax = SyntaxUtilities.GetSyntaxFromSymbol<MethodDeclarationSyntax>(symbol);
        if (syntax.Body is { } body) {
            ctx.PushScope();

            foreach (var result in body.Statements.Select(stmt => StatementBuilder.Build(stmt, ctx))) {
                functionBlock.AddStatements(result.Statements);
            }

            ctx.PopScope();
        }

        return new FunctionDeclarationStatement {
            Name = FunctionName.FromString(cls.Name + ":" + symbol.Name),
            Body = new FunctionBody {
                Body = functionBlock,
                Parameters = pars,
                TypeSpecifiers = specs,
                ReturnType = returnType,
            },
        };
    }
}