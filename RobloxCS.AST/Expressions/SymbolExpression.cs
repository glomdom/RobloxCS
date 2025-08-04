namespace RobloxCS.AST.Expressions;

public sealed class SymbolExpression : Expression {
    public required string Value { get; set; }

    public static SymbolExpression FromString(string name) => new() { Value = name };
}