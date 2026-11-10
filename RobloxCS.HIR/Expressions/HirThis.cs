namespace RobloxCS.HIR.Expressions;

public sealed record HirThis : HirExpression {
    public bool IsBase { get; init; }
}