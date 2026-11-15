using System.Collections.Immutable;

namespace RobloxCS.HIR.Expressions;

public sealed record HirCollectionLiteral : HirExpression {
    public HirExpression? Length { get; init; }
    public required ImmutableArray<HirExpression> Elements { get; init; }
}