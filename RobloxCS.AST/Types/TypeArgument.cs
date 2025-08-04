namespace RobloxCS.AST.Types;

public sealed class TypeArgument : AstNode {
    public string? Name;
    public required TypeInfo TypeInfo;

    public bool HasName => Name is not null;

    public static TypeArgument From(string name, TypeInfo info) => new TypeArgument { Name = name, TypeInfo = info };
}