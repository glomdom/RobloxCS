using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Builders;

public record BuilderResult {
    public List<Statement> Statements { get; }

    private BuilderResult(IEnumerable<Statement> statements) {
        Statements = statements.ToList();
    }

    public void AddStatement(Statement stmt) => Statements.Add(stmt);
    public void AddStatements(List<Statement> stmts) => Statements.AddRange(stmts);

    public static BuilderResult FromSingle(Statement stmt) => new([stmt]);
    public static BuilderResult From(List<Statement> stmts) => new(stmts);
    public static BuilderResult Empty() => new([]);

    public void Add(BuilderResult other) => Statements.AddRange(other.Statements);
}