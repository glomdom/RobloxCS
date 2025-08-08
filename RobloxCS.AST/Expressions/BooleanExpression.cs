namespace RobloxCS.AST.Expressions;

public sealed class BooleanExpression : Expression {
    public required bool Value { get; set; }

    public override BooleanExpression DeepClone() => new() { Value = Value };
}