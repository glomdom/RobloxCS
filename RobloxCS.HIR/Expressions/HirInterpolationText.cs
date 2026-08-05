namespace RobloxCS.HIR.Expressions;

public sealed record HirInterpolationText : HirInterpolationPart {
    public required string Value { get; init; }
}