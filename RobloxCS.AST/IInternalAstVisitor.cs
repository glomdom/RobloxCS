using RobloxCS.AST.Transient;

namespace RobloxCS.AST;

public interface IInternalAstVisitor {
    void VisitTransientForLoop(TransientForLoop node);
}

public interface IInternalAstVisitor<out T> : IAstVisitor<T> where T : AstNode {
    T VisitTransientForLoop(TransientForLoop node);
}