namespace RobloxCS.HIR.Expressions;

public sealed record HirCoalesce : HirExpression {
    public required HirExpression Value { get; init; }
    public required HirExpression WhenNull { get; init; }
}