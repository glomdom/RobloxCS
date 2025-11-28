namespace RobloxCS.AST.Expressions;

public sealed class ElseIfExpression : Expression {
    public required Expression Condition { get; set; }
    public required Expression Expression { get; set; }

    public static ElseIfExpression From(Expression cond, Expression expr) => new() { Condition = cond, Expression = expr };

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitElseIfExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitElseIfExpression(this);

    public override IEnumerable<AstNode> Children() {
        yield return Condition;
        yield return Expression;
    }
}