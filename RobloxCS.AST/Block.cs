using RobloxCS.AST.Statements;

namespace RobloxCS.AST;

public sealed class Block : AstNode {
    public required List<Statement> Statements { get; set; }

    public static Block Empty() => new() { Statements = [] };
    public static Block From(Statement stmt) => new() { Statements = [stmt] };
    public static Block From(List<Statement> stmts) => new() { Statements = stmts };

    public void AddStatement(Statement statement) => Statements.Add(statement);
    public void AddStatements(List<Statement> stmts) => Statements.AddRange(stmts);

    /// <summary>
    /// Flattens the provided block into the block.
    /// </summary>
    public void AddBlock(Block block) => Statements.AddRange(block.Statements);

    public override Block DeepClone() => new() { Statements = Statements.Select(s => (Statement)s.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitBlock(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBlock(this);

    public override IEnumerable<Statement> Children() => Statements;
}