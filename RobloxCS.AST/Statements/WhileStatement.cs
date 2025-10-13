using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public class WhileStatement : Statement {
    public required Expression Condition { get; set; }
    public required Block Block { get; set; }

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitWhileStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitWhileStatement(this);

    public override IEnumerable<AstNode> Children() {
        yield return Condition;
        yield return Block;
    }
}