namespace RobloxCS.AST.Types;

public sealed class TableTypeInfo : TypeInfo {
    public required List<TypeField> Fields { get; set; }

    public static TableTypeInfo Empty() => new() { Fields = [] };
}