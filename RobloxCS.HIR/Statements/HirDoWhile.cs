using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirDoWhile : HirStatement {
    public required HirBlock Body { get; init; }
    public required HirExpression Condition { get; init; }
}