namespace RobloxCS.HIR.Expressions;

public sealed record HirCollectionLiteral : HirExpression {
    public required List<HirExpression> Elements { get; init; }
}