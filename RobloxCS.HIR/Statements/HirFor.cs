using System.Collections.Immutable;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirFor : HirStatement {
    public required ImmutableArray<HirStatement> Initializers { get; init; }
    public required HirExpression? Condition { get; init; }
    public required ImmutableArray<HirStatement> Incrementors { get; init; }
    public required HirBlock Body { get; init; }
}