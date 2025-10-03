namespace RobloxCS.Transpiler;

public enum ExpressionContext {
    Default,
    Return,
    Assignment,
    Argument,
    Condition,
}