using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirExpressionStatement : HirStatement {
    public required HirExpression Expression { get; init; }
}