namespace RobloxCS.AST.Statements;

public sealed class FunctionCall : Statement {
    public required Prefix Prefix { get; set; }
    public required List<Suffix> Suffixes { get; set; }

    public override FunctionCall DeepClone() => new() {
        Prefix = (Prefix)Prefix.DeepClone(),
        Suffixes = Suffixes.Select(s => (Suffix)s.DeepClone()).ToList()
    };
}