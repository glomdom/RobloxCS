using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Suffixes;

public sealed class Dot : IndexSuffix {
    public required SymbolExpression Name { get; set; }

    public override Dot DeepClone() => new() { Name = Name.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitDot(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitDot(this);

    public override string ToString() => Name.Value;
}