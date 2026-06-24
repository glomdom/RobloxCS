namespace RobloxCS.AST.Types;

public sealed class TableTypeInfo : TypeInfo {
    public required List<TypeField> Fields { get; set; }

    public static TableTypeInfo Empty() => new() { Fields = [] };

    public override TableTypeInfo DeepClone() => new() { Fields = Fields.Select(f => f.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitTableTypeInfo(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTableTypeInfo(this);

    public override IEnumerable<AstNode> Children() {
        return Fields;
    }
}