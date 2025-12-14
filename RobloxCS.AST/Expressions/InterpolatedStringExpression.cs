namespace RobloxCS.AST.Expressions;

public sealed class InterpolatedStringExpression : Expression {
    public required List<InterpolatedStringSegment> Segments { get; set; }
    public required string LastString { get; set; }

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitInterpolatedStringExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitInterpolatedStringExpression(this);
}

public sealed class InterpolatedStringSegment : AstNode {
    public required string Literal { get; set; }
    public required Expression Expression { get; set; }

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitInterpolatedStringSegment(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitInterpolatedStringSegment(this);
}