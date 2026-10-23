using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Types;

public sealed class TypeofTypeInfo : TypeInfo {
    public Expression Expression { get; set; }

    public override TypeofTypeInfo DeepClone() => new() { Expression = (Expression)Expression.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitTypeofTypeInfo(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTypeofTypeInfo(this);

    public override IEnumerable<AstNode> Children() {
        yield return Expression;
    }
}