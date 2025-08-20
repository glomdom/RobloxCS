namespace RobloxCS.AST.Expressions;

public sealed class BooleanExpression : Expression {
    public required bool Value { get; set; }

    public override BooleanExpression DeepClone() => new() { Value = Value };
    public override void Accept(IAstVisitor v) => v.VisitBooleanExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBooleanExpression(this);
}