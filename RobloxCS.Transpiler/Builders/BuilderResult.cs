using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Builders;

public record BuilderResult {
    public List<Statement> Statements { get; }

    private BuilderResult(IEnumerable<Statement> statements) {
        Statements = statements.ToList();
    }

    public static BuilderResult FromSingle(Statement stmt) => new([stmt]);
}