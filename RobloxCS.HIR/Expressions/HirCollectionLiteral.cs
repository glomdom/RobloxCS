using System.Collections.Immutable;

namespace RobloxCS.HIR.Expressions;

public sealed record HirCollectionLiteral : HirExpression {
    public required IImmutableList<HirExpression> Elements { get; init; }
}