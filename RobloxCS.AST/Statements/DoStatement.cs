namespace RobloxCS.AST.Statements;

public class DoStatement : Statement {
    public required Block Block { get; set; }

    public override DoStatement DeepClone() => new() { Block = Block.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitDoStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitDoStatement(this);

    public override IEnumerable<AstNode> Children() {
        yield return Block;
    }
}