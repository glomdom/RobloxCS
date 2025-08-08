namespace RobloxCS.AST;

public sealed class FunctionArgs : AstNode {
    public required List<Expression> Arguments { get; set; }

    public static FunctionArgs Empty() {
        return new FunctionArgs { Arguments = [] };
    }

    public override FunctionArgs DeepClone() => new() { Arguments = Arguments.Select(arg => (Expression)arg.DeepClone()).ToList() };
}