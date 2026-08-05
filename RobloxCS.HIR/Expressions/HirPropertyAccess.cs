using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirPropertyAccess : HirExpression {
    public required IPropertySymbol Symbol { get; init; }
    public required HirExpression? Receiver { get; init; }
}