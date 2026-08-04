using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR;

public abstract record HirNode {
    public required Location Location { get; init; }
}