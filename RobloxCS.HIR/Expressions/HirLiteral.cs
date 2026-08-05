namespace RobloxCS.HIR.Expressions;

public sealed record HirLiteral : HirExpression {
    public required object? Value { get; init; }
}