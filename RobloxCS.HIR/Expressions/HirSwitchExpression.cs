using System.Collections.Immutable;
using RobloxCS.HIR.Patterns;

namespace RobloxCS.HIR.Expressions;

public sealed record HirSwitchExpression : HirExpression {
    public required HirExpression Value { get; init; }
    public required ImmutableArray<HirSwitchArm> Arms { get; init; }
}

public sealed record HirSwitchArm : HirNode {
    public required HirPattern Pattern { get; init; }
    public HirExpression? Guard { get; init; }
    public required HirExpression Value { get; init; }
}
