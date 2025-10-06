namespace RobloxCS.AST.Statements;

public sealed class ContinueStatement : Statement {
    public override ContinueStatement DeepClone() => new();

    public override void Accept(IAstVisitor v) => v.VisitContinueStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitContinueStatement(this);
}