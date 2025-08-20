namespace RobloxCS.AST.Expressions;

public sealed class NumberExpression : Expression {
    public required double Value { get; set; }

    public override NumberExpression DeepClone() => new() { Value = Value };
    public override void Accept(IAstVisitor v) => v.VisitNumberExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitNumberExpression(this);
}