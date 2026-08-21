using System.Collections.Immutable;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirFor : HirStatement {
    public required IImmutableList<HirStatement> Initializers { get; init; }
    public required HirExpression? Condition { get; init; }
    public required IImmutableList<HirStatement> Incrementors { get; init; }
    public required HirBlock Body { get; init; }
}