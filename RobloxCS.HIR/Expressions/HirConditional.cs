namespace RobloxCS.HIR.Expressions;

public sealed record HirConditional : HirExpression {
    public required HirExpression Condition { get; init; }
    public required HirExpression WhenTrue { get; init; }
    public required HirExpression WhenFalse { get; init; }
}