using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Patterns;

public sealed record HirConstantPattern : HirPattern {
    public required HirExpression Value { get; init; }
}
