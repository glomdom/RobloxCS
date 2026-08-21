using System.Collections.Immutable;

namespace RobloxCS.HIR.Expressions;

public sealed record HirInterpolatedString : HirExpression {
    public required IImmutableList<HirInterpolationPart> Parts { get; init; }
}