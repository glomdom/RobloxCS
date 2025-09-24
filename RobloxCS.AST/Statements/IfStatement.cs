using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class IfStatement : Statement {
    public required Expression Condition { get; set; }
    public required Block Block { get; set; }
    public List<ElseIfBlock>? ElseIf { get; set; }
    public Block? Else { get; set; }

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitIfStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitIfStatement(this);
}