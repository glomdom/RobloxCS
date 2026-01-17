namespace RobloxCS.AST.Types;

public sealed class TypeArgument : AstNode {
    public string? Name;
    public required TypeInfo TypeInfo;

    public bool HasName => Name is not null;

    public static TypeArgument From(string name, TypeInfo info) => new() { Name = name, TypeInfo = info };

    public override TypeArgument DeepClone() => new() { Name = Name, TypeInfo = (TypeInfo)TypeInfo.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitTypeArgument(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTypeArgument(this);

    public override IEnumerable<AstNode> Children() {
        yield return TypeInfo;
    }
}