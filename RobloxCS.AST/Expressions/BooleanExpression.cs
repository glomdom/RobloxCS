namespace RobloxCS.AST.Expressions;

public sealed class BooleanExpression : Expression {
    public bool Value { get; set; }

    public BooleanExpression(bool value) {
        Value = value;
    }

    public override string ToString() => Value.ToString().ToLowerInvariant();
}