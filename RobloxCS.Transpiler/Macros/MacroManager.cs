using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Expressions;
using RobloxCS.Transpiler.Helpers;

namespace RobloxCS.Transpiler.Macros;

public delegate TResult MacroHandler<in TNode, out TResult>(TNode syntax, TranspilationContext ctx)
    where TNode : SyntaxNode;

public static class MacroManager {
    private static readonly Dictionary<string, MacroHandler<InvocationExpressionSyntax, Expression>> CorlibMethodMacros = new() {
        { "System.Console.WriteLine", HandleConsoleWrites },
        { "System.Console.Write", HandleConsoleWrites },
    };

    private static readonly SymbolDisplayFormat MacroKeyFormat = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        memberOptions: SymbolDisplayMemberOptions.IncludeContainingType
    );

    public static string GetMacroKey(ISymbol symbol) {
        return symbol.ToDisplayString(MacroKeyFormat);
    }

    public static bool TryGetMethodMacro(string key, [NotNullWhen(true)] out MacroHandler<InvocationExpressionSyntax, Expression>? handler) {
        return CorlibMethodMacros.TryGetValue(key, out handler);
    }

    private static Expression HandleConsoleWrites(InvocationExpressionSyntax syntax, TranspilationContext ctx) {
        return ExpressionHelpers.SimpleFunctionCall("print", syntax.ArgumentList, ctx);
    }
}