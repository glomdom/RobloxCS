using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirParameterRef : HirExpression {
    public required IParameterSymbol Symbol { get; init; }
}