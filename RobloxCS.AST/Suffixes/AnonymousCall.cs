namespace RobloxCS.AST.Suffixes;

public sealed class AnonymousCall : Call {
    public required FunctionArgs Arguments { get; set; }

    public override AnonymousCall DeepClone() => new() { Arguments = Arguments.DeepClone() };
}