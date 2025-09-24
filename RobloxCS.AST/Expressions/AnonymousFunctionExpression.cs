using RobloxCS.AST.Functions;

namespace RobloxCS.AST.Expressions;

public sealed class AnonymousFunctionExpression : Expression {
    public required FunctionBody Body { get; set; }

    public override AnonymousFunctionExpression DeepClone() => new() { Body = Body.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitAnonymousFunction(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitAnonymousFunction(this);

    public override IEnumerable<AstNode> Children() {
        yield return Body;
    }
}