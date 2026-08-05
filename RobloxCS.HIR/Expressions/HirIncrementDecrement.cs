namespace RobloxCS.HIR.Expressions;

public sealed record HirIncrementDecrement : HirExpression {
    public required HirExpression Target { get; init; }
    public required bool IsPrefix { get; init; }
    public required bool IsIncrement { get; init; }
}