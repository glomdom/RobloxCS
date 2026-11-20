using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Patterns;

public sealed record HirRecursivePattern : HirPattern {
    public ITypeSymbol? MatchedType { get; init; }
    public ILocalSymbol? Declared { get; init; }
    public required ImmutableArray<HirPropertySubpattern> Properties { get; init; }
}

public sealed record HirPropertySubpattern : HirNode {
    public required HirExpression Member { get; init; }
    public required HirPattern Pattern { get; init; }
}
