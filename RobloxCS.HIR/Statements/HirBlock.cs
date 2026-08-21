using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Statements;

public sealed record HirBlock : HirStatement {
    public required IImmutableList<HirStatement> Statements { get; init; }
    public required IImmutableList<ILocalSymbol> Locals { get; init; }
}