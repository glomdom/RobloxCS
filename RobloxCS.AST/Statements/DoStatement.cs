namespace RobloxCS.AST.Statements;

public class DoStatement : Statement {
    public required Block Block { get; set; }

    public static DoStatement FromBlock(Block block) {
        return new DoStatement { Block = block };
    }

    public override DoStatement DeepClone() => new() { Block = Block.DeepClone() };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}