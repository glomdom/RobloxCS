namespace RobloxCS.AST;

public abstract class Parameter : AstNode;

public sealed class EllipsisParameter : Parameter {
    public override EllipsisParameter DeepClone() => new();
}

public sealed class NameParameter : Parameter {
    public required string Name { get; set; }

    public override NameParameter DeepClone() => new() { Name = Name };
}