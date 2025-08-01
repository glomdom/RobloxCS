namespace RobloxCS.AST.Types;

public abstract class TypeFieldKey : AstNode;

public sealed class NameTypeFieldKey : TypeFieldKey {
    public required string Name { get; set; }

    public static NameTypeFieldKey FromString(string name) => new() { Name = name };
}

public sealed class IndexSignatureTypeFieldKey : TypeFieldKey {
    public required TypeInfo Inner { get; set; }

    public static IndexSignatureTypeFieldKey FromInfo(TypeInfo info) => new() { Inner = info };
}