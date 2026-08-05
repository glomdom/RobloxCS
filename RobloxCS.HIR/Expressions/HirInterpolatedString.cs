namespace RobloxCS.HIR.Expressions;

public sealed record HirInterpolatedString : HirExpression {
    public required List<HirInterpolationPart> Parts { get; init; }
}