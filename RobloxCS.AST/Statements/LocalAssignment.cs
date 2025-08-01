using System.Text;
using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class LocalAssignment : Statement {
    public List<SymbolExpression> Names { get; set; }
    public List<Expression> Expressions { get; set; }

    public LocalAssignment(
        List<SymbolExpression> names,
        List<Expression> expressions
    ) {
        Names = names;
        Expressions = expressions;
    }

    public static LocalAssignment Naked(string name) {
        return new LocalAssignment([new SymbolExpression(name)], []);
    }

    public override string ToString() {
        var sb = new StringBuilder();
        sb.Append("local ");
        sb.Append(string.Join(", ", Names));

        if (Expressions.Count == 0) return sb.ToString();
        sb.Append(" = ");
        sb.Append(string.Join(", ", Expressions));

        return sb.ToString();
    }
}