namespace RobloxCS.AST.Types;

public abstract class TypeFieldKey : AstNode;

public sealed class NameTypeFieldKey : TypeFieldKey {
    public required string Name { get; set; }

    public static NameTypeFieldKey FromString(string name) => new() { Name = name };

    public override NameTypeFieldKey DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.VisitNameTypeFieldKey(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitNameTypeFieldKey(this);
}

public sealed class IndexSignatureTypeFieldKey : TypeFieldKey {
    public required TypeInfo Inner { get; set; }

    public static IndexSignatureTypeFieldKey FromInfo(TypeInfo info) => new() { Inner = info };

    public override IndexSignatureTypeFieldKey DeepClone() => new() { Inner = (TypeInfo)Inner.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitIndexSignatureFieldKey(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitIndexSignatureFieldKey(this);

    public override IEnumerable<AstNode> Children() {
        yield return Inner;
    }
}