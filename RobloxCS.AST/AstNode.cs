namespace RobloxCS.AST;

public abstract class AstNode {
    public AstNode? Parent;
    
    public abstract AstNode DeepClone();

    public abstract void Accept(IAstVisitor v);
    public abstract T Accept<T>(IAstVisitor<T> v) where T : AstNode;

    public virtual IEnumerable<AstNode> Children() {
        yield break;
    }
}