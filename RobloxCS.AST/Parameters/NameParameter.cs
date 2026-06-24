namespace RobloxCS.AST.Parameters;

public sealed class NameParameter : Parameter {
    public required string Name { get; set; }

    public static NameParameter FromString(string name) => new() { Name = name };

    public override NameParameter DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.VisitNameParameter(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitNameParameter(this);
}