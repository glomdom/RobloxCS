namespace RobloxCS.AST.Types;

public sealed class BasicTypeInfo : TypeInfo {
    public required string Name { get; set; }

    public static BasicTypeInfo FromString(string name) => new() { Name = name };

    public static BasicTypeInfo Void() => new() { Name = "()" };
    public static BasicTypeInfo String() => new() { Name = "string" };
    public static BasicTypeInfo Number() => new() { Name = "number" };
    public static BasicTypeInfo Boolean() => new() { Name = "boolean" };

    public override BasicTypeInfo DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.VisitBasicTypeInfo(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBasicTypeInfo(this);

    public override string ToString() => $"BasicType({Name})";
}