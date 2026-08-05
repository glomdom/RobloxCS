namespace RobloxCS.HIR.Statements;

public sealed record HirBlock : HirStatement {
    public required List<HirStatement> Statements { get; init; }
}