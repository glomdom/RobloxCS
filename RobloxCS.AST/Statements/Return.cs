namespace RobloxCS.AST.Statements;

public sealed class Return : Statement {
    public List<Expression> Returns;

    public Return(List<Expression> returns) {
        Returns = returns;
    }

    public override string ToString() => $"return {string.Join(", ", Returns)}";
}