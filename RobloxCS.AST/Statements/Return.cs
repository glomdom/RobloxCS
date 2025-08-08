namespace RobloxCS.AST.Statements;

public sealed class Return : Statement {
    public required List<Expression> Returns;

    public static Return FromExpressions(List<Expression> expressions) => new() { Returns = expressions };
    public static Return Empty() => new() { Returns = [] };

    public override Return DeepClone() => new() { Returns = Returns.Select(ret => (Expression)ret.DeepClone()).ToList() };
}