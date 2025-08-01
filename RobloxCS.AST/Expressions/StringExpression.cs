namespace RobloxCS.AST.Expressions;

public sealed class StringExpression : Expression {
    public string Value { get; set; }

    public StringExpression(string value) {
        Value = value;
    }

    public override string ToString() => $"\"{Value}\"";
}