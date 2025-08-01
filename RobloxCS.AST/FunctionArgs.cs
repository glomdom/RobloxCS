namespace RobloxCS.AST;

public sealed class FunctionArgs : AstNode {
    public List<Expression> Arguments { get; set; }

    public FunctionArgs(List<Expression> arguments) {
        Arguments = arguments;
    }

    public static FunctionArgs Empty() {
        return new FunctionArgs([]);
    }
    
    public override string ToString() => $"({string.Join(", ", Arguments)})";
}