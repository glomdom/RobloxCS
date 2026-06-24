namespace RobloxCS.AST.Types;

public sealed class StringTypeInfo : TypeInfo {
    public required string Value { get; set; }

    public static StringTypeInfo FromString(string value) => new() { Value = value };

    public override StringTypeInfo DeepClone() => new() { Value = Value };
    public override void Accept(IAstVisitor v) => v.VisitStringTypeInfo(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitStringTypeInfo(this);
}