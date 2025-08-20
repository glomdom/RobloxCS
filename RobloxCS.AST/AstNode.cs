namespace RobloxCS.AST;

public abstract class AstNode {
    public abstract AstNode DeepClone();

    public abstract void Accept(IAstVisitor visitor);
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}