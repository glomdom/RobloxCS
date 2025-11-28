namespace RobloxCS.AST.Expressions;

public sealed class IfExpression : Expression {
    public required Expression Condition { get; set; }
    public required Expression If { get; set; }
    public List<ElseIfExpression>? ElseIfExpressions { get; set; }
    public required Expression Else { get; set; }

    public static IfExpression From(Expression cond, Expression ifExpr, Expression elseExpr) => new() { Condition = cond, If = ifExpr, Else = elseExpr };

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitIfExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitIfExpression(this);

    public override IEnumerable<AstNode> Children() {
        yield return Condition;
        yield return If;

        if (ElseIfExpressions is not null) {
            foreach (var eie in ElseIfExpressions) yield return eie;
        }
        
        yield return Else;
    }
}