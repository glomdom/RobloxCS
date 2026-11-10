using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Statements;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Declarations;

public sealed record HirMethod : HirDeclaration {
    public HirCall? Initializer { get; init; }

    public required IMethodSymbol Symbol { get; init; }

    public required ImmutableArray<HirParameter> Parameters { get; init; }
    public required ImmutableArray<ITypeParameterSymbol> TypeParameters { get; init; }
    public required HirBlock? Block { get; init; }
    public required bool IsStatic { get; init; }
    public required bool IsConstructor { get; init; }
    public required bool IsEntryPoint { get; init; }
    public required bool IsImplicit { get; init; }
}