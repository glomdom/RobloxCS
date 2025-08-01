namespace RobloxCS.AST.Types;

public sealed class TypeField : AstNode {
    public AccessModifier? Access { get; set; }
    public required TypeFieldKey Key { get; set; }
    public required TypeInfo Value { get; set; }
}