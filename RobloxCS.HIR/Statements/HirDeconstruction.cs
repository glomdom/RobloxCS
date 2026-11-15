using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirDeconstruction : HirStatement {
    public required ImmutableArray<HirDeconstructionTarget> Targets { get; init; }
    public required HirExpression Value { get; init; }
}

public sealed record HirDeconstructionTarget : HirNode {
    public ITypeSymbol? Type { get; init; }
    public ILocalSymbol? Declared { get; init; }
    public HirExpression? Target { get; init; }
}
