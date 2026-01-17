namespace RobloxCS.AST.Types;

public sealed class UnionTypeInfo : TypeInfo {
    public required List<TypeInfo> Types;

    public static UnionTypeInfo FromTypes(List<TypeInfo> types) => new() { Types = types };
    public static UnionTypeInfo FromInfos(params TypeInfo[] types) => new() { Types = [..types] };

    public static UnionTypeInfo FromNames(params string[] typeNames) => new() {
        Types = typeNames.Select(BasicTypeInfo.FromString).Cast<TypeInfo>().ToList(),
    };

    public override UnionTypeInfo DeepClone() => new() { Types = Types.Select(t => (TypeInfo)t.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitUnionTypeInfo(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitUnionTypeInfo(this);

    public override IEnumerable<AstNode> Children() {
        foreach (var t in Types) yield return t;
    }
}