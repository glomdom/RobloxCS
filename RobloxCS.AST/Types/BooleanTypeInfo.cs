namespace RobloxCS.AST.Types;

public sealed class BooleanTypeInfo : TypeInfo {
    public required bool Value { get; set; }
    
    public static BooleanTypeInfo FromBoolean(bool value) => new() { Value = value };
}