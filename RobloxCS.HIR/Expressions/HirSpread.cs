using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirSpread : HirExpression {
    public required HirExpression Operand { get; init; }
    public required ITypeSymbol ElementType { get; init; }
}