namespace RobloxCS.AST.Statements;

public abstract class Statement : AstNode {
    public override void Accept(IAstVisitor v) => v.VisitStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitStatement(this);
}