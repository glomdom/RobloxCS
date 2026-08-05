using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirAssignment : HirStatement {
    public required HirExpression Target { get; init; }
    public required HirExpression Value { get; init; }
}