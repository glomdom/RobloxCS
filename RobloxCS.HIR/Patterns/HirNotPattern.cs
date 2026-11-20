namespace RobloxCS.HIR.Patterns;

public sealed record HirNotPattern : HirPattern {
    public required HirPattern Pattern { get; init; }
}
