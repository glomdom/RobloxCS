namespace RobloxCS.AST.Types;

public sealed class TypeField : AstNode {
    public AccessModifier? Access { get; set; }
    public required TypeFieldKey Key { get; set; }
    public required TypeInfo Value { get; set; }

    public static TypeField FromNameAndType(string name, TypeInfo type) => new() { Key = NameTypeFieldKey.FromString(name), Value = type };
}