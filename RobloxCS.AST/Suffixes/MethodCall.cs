using RobloxCS.AST.Functions;

namespace RobloxCS.AST.Suffixes;

public sealed class MethodCall : Call {
    public required string Name { get; set; }
    public required FunctionArgs Args { get; set; }

    public override MethodCall DeepClone() => new() { Name = Name, Args = Args.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitMethodCall(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitMethodCall(this);
}