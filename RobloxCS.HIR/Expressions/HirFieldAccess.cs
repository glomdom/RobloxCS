using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirFieldAccess : HirExpression {
    public required IFieldSymbol Symbol { get; init; }
    public required HirExpression? Receiver { get; init; }
}