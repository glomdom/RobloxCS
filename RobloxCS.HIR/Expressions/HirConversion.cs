using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirConversion : HirExpression {
    public required HirExpression Operand { get; init; }
    public required ITypeSymbol TargetType { get; init; }
    public required HirConversionKind Kind { get; init; }
    public required IMethodSymbol? Method { get; init; }
}