using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class CompoundAssignmentStatement : Statement {
    public required Var Right { get; set; }
    public required CompoundOp Operator { get; set; }
    public required Expression Left { get; set; }

    public override CompoundAssignmentStatement DeepClone() => new() {
        Right = (Var)Right.DeepClone(),
        Operator = Operator,
        Left = (Expression)Left.DeepClone(),
    };
    
    public override void Accept(IAstVisitor v) => v.VisitCompoundAssignmentStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitCompoundAssignmentStatement(this);

    public override IEnumerable<AstNode> Children() {
        yield return Right;
        yield return Left;
    }
}