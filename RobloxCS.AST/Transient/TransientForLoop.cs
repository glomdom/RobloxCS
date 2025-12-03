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
}