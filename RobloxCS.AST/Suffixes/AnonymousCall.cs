using RobloxCS.AST.Functions;

namespace RobloxCS.AST.Suffixes;

public sealed class AnonymousCall : Call {
    public required FunctionArgs Arguments { get; set; }

    public static AnonymousCall FromArgs(FunctionArgs args) => new() { Arguments = args };

    public override AnonymousCall DeepClone() => new() { Arguments = Arguments.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitAnonymousCall(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitAnonymousCall(this);

    public override IEnumerable<AstNode> Children() {
        yield return Arguments;
    }
}