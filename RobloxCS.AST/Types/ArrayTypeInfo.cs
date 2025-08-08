namespace RobloxCS.AST.Types;

public sealed class ArrayTypeInfo : TypeInfo {
    public AccessModifier? Access { get; set; }
    public required TypeInfo ElementType { get; set; }

    public override ArrayTypeInfo DeepClone() => new() { Access = Access, ElementType = (TypeInfo)ElementType.DeepClone() };
}

public enum AccessModifier {
    Read,
    Write,
}