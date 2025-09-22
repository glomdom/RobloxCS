using RobloxCS.AST.Types;

namespace RobloxCS.AST.Expressions;

public class TypeAssertionExpression : Expression {
    public required Expression Expression { get; set; }
    public required TypeInfo AssertTo { get; set; }

    public static TypeAssertionExpression From(Expression expr, TypeInfo @as) => new() { Expression = expr, AssertTo = @as };

    public override TypeAssertionExpression DeepClone() => new() { Expression = (Expression)Expression.DeepClone(), AssertTo = (TypeInfo)AssertTo.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitTypeAssertionExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTypeAssertionExpression(this);
}