namespace RobloxCS.AST.Expressions;

public sealed class ParenthesisExpression : Expression {
    public required Expression Expression { get; set; }

    public static ParenthesisExpression From(Expression expr) => new() { Expression = expr };

    public override ParenthesisExpression DeepClone() => new() { Expression = (Expression)Expression.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitParenthesisExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitParenthesisExpression(this);

    public override IEnumerable<AstNode> Children() {
        yield return Expression;
    }

    public override string ToString() => $"Parenthesis({Expression})";
}