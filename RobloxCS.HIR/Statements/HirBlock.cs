using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Statements;

public sealed record HirBlock : HirStatement {
    public required ImmutableArray<HirStatement> Statements { get; init; }
    public required ImmutableArray<ILocalSymbol> Locals { get; init; }
}