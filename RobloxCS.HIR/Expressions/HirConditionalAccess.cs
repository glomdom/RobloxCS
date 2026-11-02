namespace RobloxCS.HIR.Expressions;

public sealed record HirConditionalAccess : HirExpression {
    public required HirExpression Receiver { get; init; }
    public required HirExpression WhenNotNull { get; init; }
}