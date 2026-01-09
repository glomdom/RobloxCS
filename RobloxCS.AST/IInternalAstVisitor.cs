using RobloxCS.AST.Transient;

namespace RobloxCS.AST;

internal interface IInternalAstVisitor : IAstVisitor {
    void VisitTransientForLoop(TransientForLoop node);
    void VisitTransientBlock(TransientBlock node);
    void VisitTransientServiceUsageExpression(TransientServiceUsageExpression node);
}

internal interface IInternalAstVisitor<out T> : IAstVisitor<T> where T : AstNode {
    T VisitTransientForLoop(TransientForLoop node);
    T VisitTransientBlock(TransientBlock node);
    T VisitTransientServiceUsageExpression(TransientServiceUsageExpression node);
}