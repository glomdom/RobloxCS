namespace RobloxCS.AST;

public sealed class Block : AstNode {
    public required List<Statement> Statements { get; set; }

    public static Block Empty() {
        return new Block { Statements = [] };
    }

    public Block AddStatement(Statement statement) {
        Statements.Add(statement);

        return this;
    }
}