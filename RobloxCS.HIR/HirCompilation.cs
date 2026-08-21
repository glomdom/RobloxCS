using System.Collections.Immutable;

namespace RobloxCS.HIR;

public sealed record HirCompilation : HirNode {
    public required IImmutableList<HirModule> Modules { get; init; }
}