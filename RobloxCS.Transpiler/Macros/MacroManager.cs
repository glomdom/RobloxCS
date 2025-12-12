using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;
using RobloxCS.Transpiler.Helpers;

namespace RobloxCS.Transpiler.Macros;

public delegate TResult MacroHandler<TNode, TResult>(TNode syntax, TranspilationContext ctx)
    where TNode : SyntaxNode;

public static class MacroManager {

    private static readonly Dictionary<string, MacroHandler<InvocationExpressionSyntax, Expression>> CorlibMethodMacros = new() {
        { "System.Console.WriteLine", HandleConsoleWrites },
    };

    public static bool TryGetMethodMacro(string key, [NotNullWhen(true)] out MacroHandler<InvocationExpressionSyntax, Expression>? handler) {
        return CorlibMethodMacros.TryGetValue(key, out handler);
    }

    private static Expression HandleConsoleWrites(InvocationExpressionSyntax syntax, TranspilationContext ctx) {
        return ExpressionHelpers.SimpleFunctionCall("print", syntax.ArgumentList, ctx);
    }
}