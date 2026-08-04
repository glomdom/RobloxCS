namespace RobloxCS.HIR.Statements;

public sealed record HirBlock {
    public required List<HirStatement> Statements { get; init; }
}