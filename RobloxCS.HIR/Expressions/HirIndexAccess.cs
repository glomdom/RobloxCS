using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Expressions;

public sealed record HirIndexAccess : HirExpression {
    public required HirExpression Receiver { get; init; }
    public required IImmutableList<HirArgument> Arguments { get; init; }
    public required IPropertySymbol? Indexer { get; init; }
}