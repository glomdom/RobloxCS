namespace RobloxCS.AST.Expressions;

public sealed class IfExpression : Expression {
    public required Expression Condition { get; set; }
    public required List<ElseIfExpression> ElseIfExpressions { get; set; }
    public required Expression ElseExpression { get; set; }
    
    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitIfExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitIfExpression(this);
}