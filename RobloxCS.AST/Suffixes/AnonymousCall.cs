namespace RobloxCS.AST.Suffixes;

public sealed class AnonymousCall : Call {
    public FunctionArgs Arguments { get; set; }

    public AnonymousCall(FunctionArgs args) {
        Arguments = args;
    }

    public override string ToString() => Arguments.ToString();
}