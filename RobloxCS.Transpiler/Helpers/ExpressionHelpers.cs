using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.Transpiler.Helpers;

public static class ExpressionHelpers {
    public static TableConstructorExpression EmptyTableConstructor() => new() { Fields = [] };

    public static ParenthesisExpression ParenthesisFromInner(Expression inner) => new() { Expression = inner };

    /// <summary>
    /// Creates a simple function call in the form of <c>name(...args)</c>.
    /// </summary>
    public static FunctionCallExpression SimpleFunctionCall(string name, params Expression[] args) => SimpleFunctionCall(name, args.ToList());

    /// <inheritdoc cref="SimpleFunctionCall(string, Expression[])"/>
    public static FunctionCallExpression SimpleFunctionCall(string name, List<Expression> args) {
        var prefix = NamePrefix.FromString(name);
        var arguments = FunctionArgsFromExpressions(args);

        var suffix = new AnonymousCall {
            Arguments = arguments,
        };

        var expr = new FunctionCallExpression {
            Prefix = prefix,
            Suffixes = [suffix],
        };

        return expr;
    }

    public static FunctionCallExpression SimpleMethodCall(string name, string methodName, params Expression[] args) {
        var prefix = NamePrefix.FromString(name);
        var suffix = new MethodCall { Name = methodName, Args = FunctionArgsFromExpressions(args) };

        var stmt = new FunctionCallExpression {
            Prefix = prefix,
            Suffixes = [suffix],
        };

        return stmt;
    }

    public static FunctionArgs EmptyFunctionArgs() {
        return new FunctionArgs { Arguments = [] };
    }

    public static FunctionArgs FunctionArgsFromExpressions(List<Expression> exprs) {
        var funcArgs = EmptyFunctionArgs();
        funcArgs.Arguments = exprs;

        return funcArgs;
    }

    public static FunctionArgs FunctionArgsFromExpressions(Expression[] exprs) {
        var funcArgs = EmptyFunctionArgs();
        var conv = exprs.ToList();

        funcArgs.Arguments = conv;

        return funcArgs;
    }

    public static SymbolExpression SymbolFromString(string str) => new() { Value = str };

    public static FunctionName FunctionNameFromString(string str) {
        var colonIdx = str.LastIndexOf(':');
        string? colonName = null;
        var left = str;

        if (colonIdx >= 0) {
            colonName = str[(colonIdx + 1)..];
            left = str[..colonIdx];
        }

        var names = left.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();

        return new FunctionName {
            Names = names,
            ColonName = colonName,
        };
    }
}