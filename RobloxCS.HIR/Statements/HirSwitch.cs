using System.Collections.Immutable;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Patterns;

namespace RobloxCS.HIR.Statements;

public sealed record HirSwitch : HirStatement {
    public required HirExpression Value { get; init; }
    public required ImmutableArray<HirSwitchSection> Sections { get; init; }
}

public sealed record HirSwitchSection : HirNode {
    public required ImmutableArray<HirSwitchLabel> Labels { get; init; }
    public required HirBlock Body { get; init; }
}

public sealed record HirSwitchLabel : HirNode {
    public HirPattern? Pattern { get; init; }
    public HirExpression? Guard { get; init; }
}
