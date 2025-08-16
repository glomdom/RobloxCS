using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class Assignment : Statement {
    public required List<Var> Vars { get; set; }
    public required List<Expression> Expressions { get; set; }

    public static Assignment AssignToSymbol(string from, string to) {
        return new Assignment {
            Vars = [VarName.FromString(from)],
            Expressions = [SymbolExpression.FromString(to)]
        };
    }

    public override Assignment DeepClone() => new() {
        Vars = Vars.Select(v => (Var)v.DeepClone()).ToList(),
        Expressions = Expressions.Select(e => (Expression)e.DeepClone()).ToList(),
    };
}