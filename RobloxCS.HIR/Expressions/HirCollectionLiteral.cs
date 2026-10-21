using System.Collections.Immutable;

namespace RobloxCS.HIR.Expressions;

public sealed record HirCollectionLiteral : HirExpression {
    public required ImmutableArray<HirExpression> Elements { get; init; }
}