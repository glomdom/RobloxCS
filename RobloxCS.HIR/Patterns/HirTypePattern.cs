using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Patterns;

public sealed record HirTypePattern : HirPattern {
    public required ITypeSymbol TestType { get; init; }
    public ILocalSymbol? Declared { get; init; }
}
