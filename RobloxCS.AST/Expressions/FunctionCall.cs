namespace RobloxCS.AST.Expressions;

public class FunctionCall : Expression {
    public required Prefix Prefix { get; set; }
    public required List<Suffix> Suffixes { get; set; }

    public override FunctionCall DeepClone() => new() { Prefix = (Prefix)Prefix.DeepClone(), Suffixes = Suffixes.Select(s => (Suffix)s.DeepClone()).ToList() };
}