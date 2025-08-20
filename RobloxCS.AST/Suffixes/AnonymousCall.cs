namespace RobloxCS.AST.Suffixes;

public sealed class AnonymousCall : Call {
    public required FunctionArgs Arguments { get; set; }

    public override AnonymousCall DeepClone() => new() { Arguments = Arguments.DeepClone() };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}