namespace RobloxCS.AST.Prefixes;

public sealed class ExpressionPrefix : Prefix {
    public Expression Expression { get; set; }

    public ExpressionPrefix(Expression expression) {
        Expression = expression;
    }
    
    public override string ToString() => Expression.ToString();
}