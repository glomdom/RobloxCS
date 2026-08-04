using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR.Declarations;

public sealed record HirMethod : HirDeclaration {
    public required IMethodSymbol Symbol { get; init; }

    public required List<HirParameter> Parameters { get; init; }
    public required List<ITypeParameterSymbol> TypeParameters { get; init; }
    public required HirBlock? Block { get; init; }
    public required bool IsStatic { get; init; }
    public required bool IsConstructor { get; init; }
    public required bool IsEntryPoint { get; init; }
}