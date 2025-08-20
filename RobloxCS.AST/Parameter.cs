namespace RobloxCS.AST;

public abstract class Parameter : AstNode;

public sealed class EllipsisParameter : Parameter {
    public override EllipsisParameter DeepClone() => new();
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}

public sealed class NameParameter : Parameter {
    public required string Name { get; set; }

    public override NameParameter DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}