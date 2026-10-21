namespace RobloxCS.HIR.Interpolation;

public sealed record HirInterpolationText : HirInterpolationPart {
    public required string Value { get; init; }
}