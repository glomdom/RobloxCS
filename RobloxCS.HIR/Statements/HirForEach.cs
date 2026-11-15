using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirForEach : HirStatement {
    public required ILocalSymbol Variable { get; init; }
    public required HirExpression Collection { get; init; }
    public required HirBlock Body { get; init; }
}