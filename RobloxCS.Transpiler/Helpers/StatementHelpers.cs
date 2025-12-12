using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.Transpiler.Helpers;

public static class StatementHelpers {
    public static DoStatement DoFromBlock(Block block) {
        var stmt = new DoStatement { Block = block };

        return stmt;
    }

    public static TypeDeclarationStatement EmptyTableTypeDeclarationStatement(string name) {
        var table = TypeHelpers.EmptyTableType();
        var stmt = new TypeDeclarationStatement { Name = name, DeclareAs = table };

        return stmt;
    }

    public static LocalAssignmentStatement UntypedLocalAssignment(string name, Expression expr) {
        var stmtName = SymbolExpression.FromString(name);

        var stmt = new LocalAssignmentStatement {
            Names = [stmtName],
            Expressions = [expr],
            Types = [],
        };
        
        return stmt;
    }

    public static LocalAssignmentStatement SingleTypedLocalAssignment(string name, Expression expr, TypeInfo type) {
        var stmt = UntypedLocalAssignment(name, expr);
        stmt.Types = [type];
        
        return stmt;
    }

    public static ReturnStatement EmptyReturnStatement() => new() { Returns = [] };

    public static ReturnStatement SimpleReturnStatement(Expression expr) {
        var stmt = EmptyReturnStatement();
        stmt.Returns = [expr];
        
        return stmt;
    }

    public static FunctionDeclarationStatement FullFunctionDeclaration(string name, List<Parameter> pars, List<TypeInfo> types, Block body, TypeInfo returnType) {
        var funcName = ExpressionHelpers.FunctionNameFromString(name);

        var funcBody = new FunctionBody {
            Parameters = pars,
            TypeSpecifiers = types,
            Body = body,
            ReturnType = returnType,
        };

        var decl = new FunctionDeclarationStatement {
            Name = funcName,
            Body = funcBody,
        };
        
        return decl;
    }

    public static FunctionCallStatement SimpleMethodCall(string name, string methodName, params Expression[] args) {
        var prefix = NamePrefix.FromString(name);
        var suffix = new MethodCall { Name = methodName, Args = ExpressionHelpers.FunctionArgsFromExpressions(args) };

        var stmt = new FunctionCallStatement {
            Prefix = prefix,
            Suffixes = [suffix],
        };
        
        return stmt;
    }

    public static FunctionCallStatement SimpleMethodCall(string name, string methodName, FunctionArgs args) {
        var prefix = NamePrefix.FromString(name);
        var suffix = new MethodCall { Name = methodName, Args = args };

        var stmt = new FunctionCallStatement {
            Prefix = prefix,
            Suffixes = [suffix],
        };

        return stmt;
    }
}