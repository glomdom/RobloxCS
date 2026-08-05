using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace RobloxCS.HIR.Expressions;

public sealed record HirBinary : HirExpression {
    public required BinaryOperatorKind Op { get; init; }
    public required HirExpression Left { get; init; }
    public required HirExpression Right { get; init; }
    public required IMethodSymbol? OperatorMethod { get; init; }
    public required bool IsChecked { get; init; }
}