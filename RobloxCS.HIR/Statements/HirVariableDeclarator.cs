using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirVariableDeclarator : HirStatement {
    public required ILocalSymbol Symbol { get; init; }
    public required HirExpression? Initializer { get; init; }
}