namespace RobloxCS.HIR.Expressions;

public record HirInterpolationExpression : HirInterpolationPart {
    public required HirExpression Expression { get; init; }
    public required string? Format { get; init; }
}