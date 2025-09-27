namespace RobloxCS.AST.Statements;

public sealed class BreakStatement : Statement {
    public override BreakStatement DeepClone() => new();

    public override void Accept(IAstVisitor v) => v.VisitBreakStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBreakStatement(this);
}