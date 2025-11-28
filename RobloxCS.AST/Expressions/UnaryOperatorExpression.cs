namespace RobloxCS.AST.Expressions;

public sealed class UnaryOperatorExpression : Expression {
    public required UnOp UnOp { get; set; }
    public required Expression Expression { get; set; }

    public static UnaryOperatorExpression Reversed(Expression expr) => new() { Expression = expr, UnOp = UnOp.Not };

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitUnaryOperatorExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitUnaryOperatorExpression(this);

    public override IEnumerable<AstNode> Children() {
        yield return Expression;
    }
}