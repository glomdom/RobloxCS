namespace RobloxCS.AST.Prefixes;

public sealed class NamePrefix : Prefix {
    public required string Name { get; set; }

    public static NamePrefix FromString(string prefix) {
        return new NamePrefix { Name = prefix };
    }

    public override NamePrefix DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}