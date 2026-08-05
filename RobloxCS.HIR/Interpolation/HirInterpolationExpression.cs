using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Interpolation;

public record HirInterpolationExpression : HirInterpolationPart {
    public required HirExpression Expression { get; init; }
    public required string? Format { get; init; }
}