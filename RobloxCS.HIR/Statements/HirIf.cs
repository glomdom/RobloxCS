using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirIf : HirStatement {
    public required HirExpression Condition { get; init; }
    public required HirBlock Then { get; init; }
    public required HirStatement? Else { get; init; }
}