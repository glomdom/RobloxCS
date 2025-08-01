namespace RobloxCS.AST.Expressions;

public sealed class SymbolExpression : Expression {
    public string Value { get; set; }

    public SymbolExpression(string value) {
        Value = value;
    }

    public override string ToString() => Value;
}