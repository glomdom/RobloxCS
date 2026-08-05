namespace RobloxCS.HIR.Statements;

public sealed record HirLocalDeclaration : HirStatement {
    public required List<HirVariableDeclarator> Declarators { get; init; }
}