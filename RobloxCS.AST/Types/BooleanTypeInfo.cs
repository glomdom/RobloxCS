namespace RobloxCS.AST.Types;

public sealed class BooleanTypeInfo : TypeInfo {
    public required bool Value { get; set; }

    public static BooleanTypeInfo FromBoolean(bool value) => new() { Value = value };

    public override BooleanTypeInfo DeepClone() => new() { Value = Value };
    public override void Accept(IAstVisitor v) => v.VisitBooleanTypeInfo(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBooleanTypeInfo(this);
}