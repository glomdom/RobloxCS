using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;

namespace RobloxCS.AST.Transient;

/// <summary>
/// Represents a for loop in the form of: <c>for (inits; condition; iterators) { body }</c>
/// </summary>
public sealed class TransientForLoop : TransientStatement {
    public List<Statement> Initializers { get; set; } = [];
    public Expression? Condition { get; set; }
    public List<Statement> Incrementors { get; set; } = [];
    public required Block Body { get; set; }

    public override AstNode DeepClone() => throw new NotImplementedException();

    public override void Accept(IAstVisitor v) {
        if (v is IInternalAstVisitor internalVisitor) {
            internalVisitor.VisitTransientForLoop(this);
        }

        throw new Exception("Transient for loop must be lowered before lowering.");
    }

    public override T Accept<T>(IAstVisitor<T> v) {
        if (v is IInternalAstVisitor<T> internalVisitor) {
            return internalVisitor.VisitTransientForLoop(this);
        }

        throw new Exception("Transient for loop must be lowered before lowering.");
    }
}