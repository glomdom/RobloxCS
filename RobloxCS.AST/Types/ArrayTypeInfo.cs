namespace RobloxCS.AST.Types;

public sealed class ArrayTypeInfo : TypeInfo {
    public AccessModifier? Access { get; set; }
    public required TypeInfo ElementType { get; set; }
}

public enum AccessModifier {
    Read,
    Write,
}