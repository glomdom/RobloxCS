namespace RobloxCS.AST.Expressions;

public sealed class AnonymousFunction : Expression {
    public required FunctionBody Body { get; set; }

    public override AnonymousFunction DeepClone() => new() { Body = Body.DeepClone() };
}