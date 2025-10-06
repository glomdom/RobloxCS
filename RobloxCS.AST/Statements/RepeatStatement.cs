using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class RepeatStatement : Statement {
    public required Block Block { get; set; }
    public required Expression Until { get; set; }
    
    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitRepeatStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitRepeatStatement(this);
}