namespace RobloxCS.HIR.Patterns;

public sealed record HirBinaryPattern : HirPattern {
    public required bool IsAnd { get; init; }
    public required HirPattern Left { get; init; }
    public required HirPattern Right { get; init; }
}
