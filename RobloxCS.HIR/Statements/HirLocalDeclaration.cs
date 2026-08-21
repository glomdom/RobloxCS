using System.Collections.Immutable;

namespace RobloxCS.HIR.Statements;

public sealed record HirLocalDeclaration : HirStatement {
    public required IImmutableList<HirVariableDeclarator> Declarators { get; init; }
}