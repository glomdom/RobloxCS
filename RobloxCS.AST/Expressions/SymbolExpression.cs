namespace RobloxCS.AST.Expressions;

public sealed class SymbolExpression : Expression {
    public required string Value { get; set; }

    public static SymbolExpression FromString(string name) => new() { Value = name };

    public override SymbolExpression DeepClone() => new() { Value = Value };
    public override void Accept(IAstVisitor v) => v.VisitSymbolExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitSymbolExpression(this);
}