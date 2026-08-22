using System.Collections.Immutable;

namespace RobloxCS.HIR.Expressions;

public sealed record HirInterpolatedString : HirExpression {
    public required ImmutableArray<HirInterpolationPart> Parts { get; init; }
}