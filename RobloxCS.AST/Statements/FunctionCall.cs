namespace RobloxCS.AST.Statements;

public sealed class FunctionCall : Statement {
    public Prefix Prefix { get; set; }
    public List<Suffix> Suffixes { get; set; }

    public FunctionCall(Prefix prefix, List<Suffix> suffixes) {
        Prefix = prefix;
        Suffixes = suffixes;
    }

    public override string ToString() => $"{Prefix}{string.Join("", Suffixes)}";
}