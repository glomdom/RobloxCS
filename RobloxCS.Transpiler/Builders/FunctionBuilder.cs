using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using RobloxCS.Transpiler.Helpers;
using TypeInfo = RobloxCS.AST.Types.TypeInfo; // conflict with `Microsoft.CodeAnalysis.TypeInfo`

namespace RobloxCS.Transpiler.Builders;

public static class FunctionBuilder {
    public static FunctionDeclarationStatement CreateNewMethod(INamedTypeSymbol classSymbol, IMethodSymbol ctorSymbol, TranspilationContext ctx) {
        var paramNames = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => p.Name).ToList();

        var args = paramNames.Select(NameParameter.FromString).Cast<Parameter>().ToList();
        var specs = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => SyntaxUtilities.TypeInfoFromSymbol(p.Type, ctx)).Cast<TypeInfo>().ToList();

        var bodyArgExprs = paramNames.Select(SymbolExpression.FromString).Cast<Expression>().ToList();
        var bodyArgs = ExpressionHelpers.FunctionArgsFromExpressions(bodyArgExprs);
        var body = CreateNewMethodBlock(classSymbol, bodyArgs);

        // function <class>.new(...)
        var decl = StatementHelpers.FullFunctionDeclaration(
            $"{classSymbol.Name}.new",
            args,
            specs,
            body,
            BasicTypeInfo.FromString($"_Instance{classSymbol.Name}")
        );

        return decl;
    }

    private static Block CreateNewMethodBlock(INamedTypeSymbol classSymbol, FunctionArgs ctorArgs) {
        var block = BlockHelpers.Empty();

        // local self = setmetatable({}, Class)
        var setmetatableCall = ExpressionHelpers.SimpleFunctionCall("setmetatable", TableConstructorExpression.Empty(), SymbolExpression.FromString(classSymbol.Name));
        var assignSelfStmt = StatementHelpers.UntypedLocalAssignment("self", setmetatableCall);
        block.AddStatement(assignSelfStmt);

        // self:constructor(...)
        var constructorCallStmt = StatementHelpers.SimpleMethodCall("self", "constructor", ctorArgs);
        block.AddStatement(constructorCallStmt);

        // return self
        var returnSelf = StatementHelpers.SimpleReturnStatement(SymbolExpression.FromString("self"));
        block.AddStatement(returnSelf);

        return block;
    }

    public static FunctionDeclarationStatement CreateConstructor(INamedTypeSymbol classSymbol, IMethodSymbol ctorSymbol, TranspilationContext ctx) {
        var functionBlock = BlockHelpers.Empty();
        var fields = classSymbol.GetMembers().OfType<IFieldSymbol>();

        foreach (var a in TypeFieldBuilder.CreateFieldAssignmentsFromFields(fields, ctx)) {
            functionBlock.AddStatement(a);
        }

        var pars = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => NameParameter.FromString(p.Name)).Cast<Parameter>().ToList();

        var specs = ctorSymbol.IsImplicitlyDeclared
            ? []
            : ctorSymbol.Parameters.Select(p => SyntaxUtilities.TypeInfoFromSymbol(p.Type, ctx)).Cast<TypeInfo>().ToList();

        var decl = StatementHelpers.FullFunctionDeclaration(
            $"{classSymbol.Name}:constructor",
            pars,
            specs,
            functionBlock,
            BasicTypeInfo.Void()
        );

        var ctorSyntax = SyntaxUtilities.MaybeGetSyntaxFromSymbol<ConstructorDeclarationSyntax>(ctorSymbol);
        if (ctorSyntax is null) {
            // ctor doesnt exist, return just initializers

            return decl;
        }

        // populate function block
        if (ctorSyntax.Body is { } body) {
            foreach (var transpiled in body.Statements.Select(stmt => StatementBuilder.Build(stmt, ctx))) {
                functionBlock.AddStatement(transpiled);
            }
        }

        return decl;
    }

    public static Statement BuildFromMethodSymbol(IMethodSymbol symbol, TranspilationContext ctx) {
        var pars = symbol.Parameters.Select(p => NameParameter.FromString(p.Name)).Cast<Parameter>().ToList();
        var specs = symbol.Parameters.Select(p => SyntaxUtilities.TypeInfoFromSymbol(p.Type, ctx)).Cast<TypeInfo>().ToList();
        var returnType = SyntaxUtilities.TypeInfoFromSymbol(symbol.ReturnType, ctx);

        var cls = symbol.ContainingSymbol;
        if (cls is null) {
            throw new Exception("Could not find containing class symbol.");
        }

        var functionBlock = BlockHelpers.Empty();

        var syntax = SyntaxUtilities.GetSyntaxFromSymbol<MethodDeclarationSyntax>(symbol);
        if (syntax.Body is { } block) {
            foreach (var result in block.Statements.Select(stmt => StatementBuilder.Build(stmt, ctx))) {
                functionBlock.AddStatement(result);
            }
        }

        var decl = StatementHelpers.FullFunctionDeclaration(
            $"{cls.Name}:{symbol.Name}",
            pars,
            specs,
            functionBlock,
            returnType
        );

        return decl;
    }
}