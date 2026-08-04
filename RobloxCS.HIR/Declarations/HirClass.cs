using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Declarations;

public sealed record HirClass : HirDeclaration {
    public required INamedTypeSymbol Symbol { get; init; }
    public required INamedTypeSymbol? Base { get; init; }
    public required List<HirField> Fields { get; init; }
    public required List<HirMethod> Methods { get; init; }
    public required List<HirProperty> Properties { get; init; }
}