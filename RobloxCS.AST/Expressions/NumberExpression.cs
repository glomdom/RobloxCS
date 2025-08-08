namespace RobloxCS.AST.Expressions;

public sealed class NumberExpression : Expression {
    public required double Value { get; set; }

    public override NumberExpression DeepClone() => new() { Value = Value };
}