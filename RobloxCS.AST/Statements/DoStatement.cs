namespace RobloxCS.AST.Statements;

public class DoStatement : Statement {
    public required Block Block { get; set; }

    public static DoStatement FromBlock(Block block) {
        return new DoStatement { Block = block };
    }

    public override DoStatement DeepClone() => new() { Block = Block.DeepClone() };
}