namespace RobloxCS.AST.Expressions;

public sealed class BooleanExpression : Expression {
    public required bool Value { get; set; }

    public static BooleanExpression True() => new() { Value = true };
    public static BooleanExpression False() => new() { Value = false };

    public override BooleanExpression DeepClone() => new() { Value = Value };
    public override void Accept(IAstVisitor v) => v.VisitBooleanExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitBooleanExpression(this);

    public override string ToString() => $"Boolean({(Value ? "true" : "false")})";
}