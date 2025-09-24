using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class ReturnStatement : Statement {
    public required List<Expression> Returns;

    public static ReturnStatement FromExpressions(List<Expression> expressions) => new() { Returns = expressions };
    public static ReturnStatement Empty() => new() { Returns = [] };

    public override ReturnStatement DeepClone() => new() { Returns = Returns.Select(ret => (Expression)ret.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitReturn(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitReturn(this);

    public override IEnumerable<AstNode> Children() {
        return Returns;
    }
}