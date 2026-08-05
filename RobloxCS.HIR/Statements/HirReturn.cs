using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirReturn : HirStatement {
    public required HirExpression? Value { get; init; }
}