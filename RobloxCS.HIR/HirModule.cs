using RobloxCS.HIR.Declarations;

namespace RobloxCS.HIR;

public sealed record HirModule {
    public required string SourcePath { get; init; }
    public required List<HirClass> Classes { get; init; }
}