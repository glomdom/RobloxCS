using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Patterns;

public sealed record HirRelationalPattern : HirPattern {
    public required BinaryOperatorKind Op { get; init; }
    public required HirExpression Value { get; init; }
}
