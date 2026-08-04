using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public abstract record HirExpression : HirNode {
    public required ITypeSymbol Type { get; init; }
}