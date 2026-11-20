using Microsoft.CodeAnalysis;

namespace RobloxCS.HIR.Patterns;

public abstract record HirPattern : HirNode {
    public ITypeSymbol? NarrowedType { get; init; }
}
