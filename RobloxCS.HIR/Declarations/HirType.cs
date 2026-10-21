using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Declarations;

public sealed record HirType : HirDeclaration {
    public required INamedTypeSymbol Symbol { get; init; }

    public required INamedTypeSymbol? Base { get; init; }
    public required ImmutableArray<HirField> Fields { get; init; }
    public required ImmutableArray<HirMethod> Methods { get; init; }
    public required ImmutableArray<HirProperty> Properties { get; init; }
}