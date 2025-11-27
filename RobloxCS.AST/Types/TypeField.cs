namespace RobloxCS.AST.Types;

public sealed class TypeField : AstNode {
    public AccessModifier? Access { get; set; }
    public required TypeFieldKey Key { get; set; }
    public required TypeInfo Value { get; set; }

    public override TypeField DeepClone() => new() {
        Access = Access,
        Key = (TypeFieldKey)Key.DeepClone(),
        Value = (TypeInfo)Value.DeepClone(),
    };

    public override void Accept(IAstVisitor v) => v.VisitTypeField(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTypeField(this);

    public override IEnumerable<AstNode> Children() {
        yield return Key;
        yield return Value;
    }

    public override string ToString() => $"TypeField({Key}: {Value})";
}