using RobloxCS.AST;
using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Helpers;

public static class BlockHelpers {
    public static Block Empty() => new() { Statements = [] };
    public static Block From(Statement stmt) => new() { Statements = [stmt] };
    public static Block From(List<Statement> stmts) => new() { Statements = stmts };

    extension(Block block) {
        public void AddStatement(Statement statement) => block.Statements.Add(statement);
        public void AddStatements(List<Statement> stmts) => block.Statements.AddRange(stmts);

        /// <summary>
        /// Flattens the provided block into the block.
        /// </summary>
        public void AddBlock(Block other) => block.Statements.AddRange(other.Statements);
    }
}