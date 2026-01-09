using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Transient;

/// <summary>
/// Statements which preserve semantics for further passes, such as for loops.
/// Such statements cannot be visited by the renderer.
/// </summary>
public abstract class TransientExpression : Expression {
    public override void Accept(IAstVisitor v) => throw new Exception($"Transient statement {GetType().Name} must be lowered before visiting.");
    public override T Accept<T>(IAstVisitor<T> v) => throw new Exception($"Transient statement {GetType().Name} must be lowered before visiting.");
}