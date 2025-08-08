namespace RobloxCS.AST.Statements;

public sealed class Assignment : Statement {
    public required List<Var> Vars { get; set; }
    public required List<Expression> Expressions { get; set; }

    public override Assignment DeepClone() => new() {
        Vars = Vars.Select(v => (Var)v.DeepClone()).ToList(),
        Expressions = Expressions.Select(e => (Expression)e.DeepClone()).ToList(),
    };
}