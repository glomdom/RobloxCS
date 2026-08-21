using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Interpolation;

public sealed record HirInterpolationExpression : HirInterpolationPart {
    public required HirExpression Expression { get; init; }
    public required string? Format { get; init; }
}