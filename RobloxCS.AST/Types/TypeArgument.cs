namespace RobloxCS.AST.Types;

public sealed class TypeArgument : AstNode {
    public string? Name;
    public required TypeInfo TypeInfo;

    public bool HasName => Name is not null;

    public static TypeArgument From(string name, TypeInfo info) => new() { Name = name, TypeInfo = info };

    public override TypeArgument DeepClone() => new() { Name = Name, TypeInfo = (TypeInfo)TypeInfo.DeepClone() };
}