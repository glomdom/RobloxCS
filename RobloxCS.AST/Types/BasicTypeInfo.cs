namespace RobloxCS.AST.Types;

public sealed class BasicTypeInfo : TypeInfo {
    public required string Name { get; set; }

    public static BasicTypeInfo FromString(string name) => new() { Name = name };
    public static BasicTypeInfo Void() => new() { Name = "()" };
}
