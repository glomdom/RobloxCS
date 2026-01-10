using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Suffixes;

public sealed class Dot : IndexSuffix {
    public required SymbolExpression Name { get; set; }
    
    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitDot(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitDot(this);
}