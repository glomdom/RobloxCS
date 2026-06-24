namespace RobloxCS.AST.Parameters;

public sealed class EllipsisParameter : Parameter {
    public override EllipsisParameter DeepClone() => new();
    public override void Accept(IAstVisitor v) => v.VisitEllipsisParameter(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitEllipsisParameter(this);
}
