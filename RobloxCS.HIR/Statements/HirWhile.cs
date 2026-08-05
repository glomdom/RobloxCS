using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirWhile : HirStatement {
    public required HirExpression Condition { get; init; }
    public required HirBlock Body { get; init; }
}