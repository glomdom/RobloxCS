using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;

namespace RobloxCS.AST;

public sealed class ElseIfBlock : AstNode {
    public required Expression Condition { get; set; }
    public required Block Block { get; set; }

    public IfStatement? ParentIf => Parent as IfStatement;

    public override AstNode DeepClone() => throw new NotImplementedException();

    public override void Accept(IAstVisitor v) {
        throw new NotImplementedException();
    }

    public override T Accept<T>(IAstVisitor<T> v) => throw new NotImplementedException();
}