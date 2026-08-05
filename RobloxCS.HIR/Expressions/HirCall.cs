using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirCall : HirExpression {
    public required IMethodSymbol Method { get; init; }
    public required HirExpression? Receiver { get; init; }
    public required List<HirExpression> Arguments { get; init; }
    public required bool IsExtension { get; init; }
}