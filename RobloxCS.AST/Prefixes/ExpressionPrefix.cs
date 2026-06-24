using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Prefixes;

public sealed class ExpressionPrefix : Prefix {
    public required Expression Expression { get; set; }

    public override ExpressionPrefix DeepClone() => new() { Expression = (Expression)Expression.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitExpressionPrefix(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitExpressionPrefix(this);

    public override IEnumerable<AstNode> Children() {
        yield return Expression;
    }
}