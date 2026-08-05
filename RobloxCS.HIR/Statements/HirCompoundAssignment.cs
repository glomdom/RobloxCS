using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;

namespace RobloxCS.HIR.Statements;

public sealed record HirCompoundAssignment : HirStatement {
    public required HirExpression Target { get; init; }
    public required BinaryOperatorKind Op { get; init; }
    public required HirExpression Value { get; init; }
    public required IMethodSymbol? UserDefinedOperator { get; init; } // maybe we could support implicit/explicit operators itf
}