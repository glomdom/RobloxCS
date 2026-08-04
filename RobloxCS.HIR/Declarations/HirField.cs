using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Declarations;

public sealed record HirField : HirDeclaration {
    public required IFieldSymbol Symbol { get; init; }

    public required HirExpression? Initializer { get; init; }
    public required bool IsStatic { get; init; }
}