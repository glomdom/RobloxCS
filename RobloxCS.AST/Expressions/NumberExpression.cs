namespace RobloxCS.AST.Expressions;

public sealed class NumberExpression : Expression {
    public double Value { get; set; }

    public NumberExpression(double value) {
        Value = value;
    }

    public override string ToString() => $"{Value}";
}