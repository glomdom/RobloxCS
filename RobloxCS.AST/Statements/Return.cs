using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class Return : Statement {
    public required List<Expression> Returns;

    public static Return FromExpressions(List<Expression> expressions) => new() { Returns = expressions };
    public static Return Empty() => new() { Returns = [] };

    public override Return DeepClone() => new() { Returns = Returns.Select(ret => (Expression)ret.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}