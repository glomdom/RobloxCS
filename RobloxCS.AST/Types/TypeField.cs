namespace RobloxCS.AST.Types;

public sealed class TypeField : AstNode {
    public AccessModifier? Access { get; set; }
    public required TypeFieldKey Key { get; set; }
    public required TypeInfo Value { get; set; }
    
    public override TypeField DeepClone() {
        var newKey = (TypeFieldKey)Key.DeepClone();
        var newValue = (TypeInfo)Value.DeepClone();
        
        var field = new TypeField {
            Access = Access,
            Key = newKey,
            Value = newValue,
        };

        newKey.Parent = field;
        newValue.Parent = field;

        return field;
    }

    public override void Accept(IAstVisitor v) => v.VisitTypeField(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTypeField(this);

    public override IEnumerable<AstNode> Children() {
        yield return Key;
        yield return Value;
    }

    public override string ToString() => $"TypeField({Key}: {Value})";
}