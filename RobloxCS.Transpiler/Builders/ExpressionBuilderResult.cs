using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Builders;

/// <summary>
/// Represents a result from an <see cref="ExpressionBuilder"/>.
///
/// The property `Statements` will not be empty if the expression has a prolog.
/// E.g. ternary __tmp, etc. 
/// </summary>
public record ExpressionBuilderResult {
    public List<Statement> Statements { get; }
    public Expression Expression { get; set; }

    private ExpressionBuilderResult(IEnumerable<Statement> statements, Expression expr) {
        Statements = statements.ToList();
        Expression = expr;
    }

    public void AddStatement(Statement stmt) => Statements.Add(stmt);

    public static ExpressionBuilderResult FromSingle(Expression expr) => new([], expr);
}