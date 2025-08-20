namespace RobloxCS.AST.Types;

public sealed class ArrayTypeInfo : TypeInfo {
    public AccessModifier? Access { get; set; }
    public required TypeInfo ElementType { get; set; }

    public override ArrayTypeInfo DeepClone() => new() { Access = Access, ElementType = (TypeInfo)ElementType.DeepClone() };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}

public enum AccessModifier {
    Read,
    Write,
}