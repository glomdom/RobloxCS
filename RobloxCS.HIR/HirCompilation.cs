using System.Collections.Immutable;

namespace RobloxCS.HIR;

public sealed record HirCompilation : HirNode {
    public required ImmutableArray<HirModule> Modules { get; init; }
}