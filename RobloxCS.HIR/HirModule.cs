using System.Collections.Immutable;
using RobloxCS.HIR.Declarations;

namespace RobloxCS.HIR;

public sealed record HirModule : HirNode {
    public required string SourcePath { get; init; }
    public required ImmutableArray<HirType> Types { get; init; }
}