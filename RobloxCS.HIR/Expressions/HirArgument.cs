using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirArgument : HirExpression {
    public required IParameterSymbol Symbol { get; init; }
    public required HirExpression Value { get; init; }
}