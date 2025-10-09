using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class AssignmentStatement : Statement {
    public required List<Var> Vars { get; set; }
    public required List<Expression> Expressions { get; set; }

    public static AssignmentStatement AssignToSymbol(string from, string to) {
        return new AssignmentStatement {
            Vars = [VarName.FromString(from)],
            Expressions = [SymbolExpression.FromString(to)],
        };
    }

    public static AssignmentStatement AssignTo(string to, Expression value) {
        return new AssignmentStatement {
            Vars = [VarName.FromString(to)],
            Expressions = [value],
        };
    }

    public override AssignmentStatement DeepClone() => new() {
        Vars = Vars.Select(v => (Var)v.DeepClone()).ToList(),
        Expressions = Expressions.Select(e => (Expression)e.DeepClone()).ToList(),
    };

    public override void Accept(IAstVisitor v) => v.VisitAssignment(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitAssignment(this);

    public override IEnumerable<AstNode> Children() {
        foreach (var v in Vars) yield return v;
        foreach (var e in Expressions) yield return e;
    }
}