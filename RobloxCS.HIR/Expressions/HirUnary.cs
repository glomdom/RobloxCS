using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace RobloxCS.HIR.Expressions;

public sealed record HirUnary : HirExpression {
    public required UnaryOperatorKind Op { get; init; }
    public required HirExpression Operand { get; init; }
    public required IMethodSymbol? OperatorMethod { get; init; }
}