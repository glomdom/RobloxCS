using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirObjectCreation : HirExpression {
    public required IMethodSymbol Constructor { get; init; }
    public required List<HirExpression> Arguments { get; init; }
}