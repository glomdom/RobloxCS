namespace RobloxCS.AST.Prefixes;

public sealed class ExpressionPrefix : Prefix {
    public required Expression Expression { get; set; }

    public override ExpressionPrefix DeepClone() => new() { Expression = (Expression)Expression.DeepClone() };
}