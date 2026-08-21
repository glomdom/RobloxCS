using System.Collections.Immutable;

namespace RobloxCS.HIR.Statements;

public sealed record HirLocalDeclaration : HirStatement {
    public required ImmutableArray<HirVariableDeclarator> Declarators { get; init; }
}