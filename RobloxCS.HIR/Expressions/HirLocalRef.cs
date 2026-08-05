using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirLocalRef : HirExpression {
    public required ILocalSymbol Symbol { get; init; }
}