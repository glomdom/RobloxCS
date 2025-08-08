namespace RobloxCS.AST.Expressions;

public sealed class StringExpression : Expression {
    public required string Value { get; set; }

    public override StringExpression DeepClone() => new() { Value = Value };
}