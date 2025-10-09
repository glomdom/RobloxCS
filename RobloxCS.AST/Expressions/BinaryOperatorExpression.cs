namespace RobloxCS.AST.Expressions;

public sealed class BinaryOperatorExpression : Expression {
    public required Expression Left { get; set; }
    public required Expression Right { get; set; }
    public required BinOp Op { get; set; }

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitBinaryOperatorExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBinaryOperatorExpression(this);
}