namespace RobloxCS.AST.Expressions;

public sealed class AnonymousFunction : Expression {
    public required FunctionBody Body { get; set; }

    public override AnonymousFunction DeepClone() => new() { Body = Body.DeepClone() };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}