using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Patterns;

public sealed record HirVarPattern : HirPattern {
    public ILocalSymbol? Declared { get; init; }
}
