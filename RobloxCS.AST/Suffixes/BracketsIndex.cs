using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Suffixes;

public sealed class BracketsIndex : IndexSuffix {
    public required Expression Expression { get; set; }

    public override BracketsIndex DeepClone() => new() { Expression = (Expression)Expression.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitBracketsIndex(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBracketsIndex(this);
}