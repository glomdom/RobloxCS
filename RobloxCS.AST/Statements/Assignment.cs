namespace RobloxCS.AST.Statements;

public sealed class Assignment : Statement {
    public List<Var> Vars { get; set; }
    public List<Expression> Expressions { get; set; }

    public Assignment(
        List<Var> vars,
        List<Expression> expressions
    ) {
        Vars = vars;
        Expressions = expressions;
    }
}