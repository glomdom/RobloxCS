namespace RobloxCS.AST.Prefixes;

public sealed class NamePrefix : Prefix {
    public required string Name { get; set; }

    public override NamePrefix DeepClone() => new() { Name = Name };
}