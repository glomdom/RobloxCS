namespace RobloxCS.AST.Statements;

public class DoStatement : Statement {
    public required Block Block { get; set; }

    public static DoStatement From(Block block) {
        return new DoStatement { Block = block };
    }
}