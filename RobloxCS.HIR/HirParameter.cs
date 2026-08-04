using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR;

public sealed record HirParameter : HirNode {
    public required IParameterSymbol Symbol { get; init; }
    public required HirExpression? DefaultValue { get; init; }
    public required bool IsParams { get; init; }
    public required RefKind RefKind { get; init; }
}