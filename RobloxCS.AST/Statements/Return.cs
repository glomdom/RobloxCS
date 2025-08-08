namespace RobloxCS.AST.Statements;

public sealed class Return : Statement {
    public required List<Expression> Returns;

    public override Return DeepClone() => new() { Returns = Returns.Select(ret => (Expression)ret.DeepClone()).ToList() };
}