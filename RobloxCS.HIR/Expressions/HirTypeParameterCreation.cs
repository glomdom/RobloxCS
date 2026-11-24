namespace RobloxCS.HIR.Expressions;

// new T() where T has the new() constraint; Type is the type parameter
public sealed record HirTypeParameterCreation : HirExpression;
