using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

/// <summary><c>throw new SomeException(message)</c>, lowered to Luau's <c>error(message)</c>.</summary>
public sealed record HirThrow : HirStatement {
    /// <summary>The exception's message argument, when it was constructed with one.</summary>
    public required HirExpression? Message { get; init; }

    public required ITypeSymbol ExceptionType { get; init; }
}
