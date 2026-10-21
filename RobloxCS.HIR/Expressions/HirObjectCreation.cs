using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirObjectCreation : HirExpression {
    public required IMethodSymbol Constructor { get; init; }
    public required ImmutableArray<HirArgument> Arguments { get; init; }
}