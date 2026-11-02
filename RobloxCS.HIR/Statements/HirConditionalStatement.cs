using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirConditionalStatement : HirStatement {
    public required HirExpression Receiver { get; init; }
    public required HirStatement Body { get; init; }
}