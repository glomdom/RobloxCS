using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR.Declarations;

public sealed record HirProperty : HirDeclaration {
    public required IPropertySymbol Symbol { get; init; }

    public required HirBlock? Getter { get; init; }
    public required HirBlock? Setter { get; init; }
    public required HirExpression? Initializer { get; init; }
    public required bool IsAuto { get; init; }
    public required bool IsStatic { get; init; }
}