using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR.Expressions;

public sealed record HirLambda : HirExpression {
    public required IMethodSymbol Symbol { get; init; }
    public required IImmutableList<HirParameter> Parameters { get; init; }
    public required HirBlock Body { get; init; }
}