using RobloxCS.HIR.Patterns;

namespace RobloxCS.HIR.Expressions;

public sealed record HirIsPattern : HirExpression {
    public required HirExpression Operand { get; init; }
    public required HirPattern Pattern { get; init; }
}
