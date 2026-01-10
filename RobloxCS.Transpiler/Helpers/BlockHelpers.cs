using RobloxCS.AST;
using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Helpers;

public static class BlockHelpers {
    public static Block Empty() => new() { Statements = [] };
    public static Block From(Statement stmt) => new() { Statements = [stmt] };
    public static Block From(List<Statement> stmts) => new() { Statements = stmts };

    extension(Block block) {
        /// <summary>
        /// Adds a statement to the block, changing the statements parent.
        /// </summary>
        /// <param name="statement">The statement that will be added to the block.</param>
        public void AddStatement(Statement statement) {
            block.Statements.Add(statement);
            statement.Parent = block;
        }

        /// <summary>
        /// Adds each statement to the block, changing their parent.
        /// </summary>
        /// <param name="stmts">The statements which will be added to the block.</param>
        public void AddStatements(List<Statement> stmts) {
            foreach (var stmt in stmts) {
                block.AddStatement(stmt);
            }
        }

        /// <summary>
        /// Flattens the provided block into the block.
        /// </summary>
        public void AddBlock(Block other) {
            block.Statements.AddRange(other.Statements);
            block.Statements.ForEach(s => s.Parent = block);
            other.Statements.Clear();
        }
    }
}