namespace RobloxCS.AST.Expressions;

public sealed class StringExpression : Expression {
    public required string Value { get; set; }

    public static StringExpression FromString(string value) => new() { Value = value };

    public override StringExpression DeepClone() => new() { Value = Value };
    public override void Accept(IAstVisitor v) => v.VisitStringExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitStringExpression(this);
}