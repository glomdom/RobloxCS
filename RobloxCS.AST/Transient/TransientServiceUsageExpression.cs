using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Transient;

public sealed class TransientServiceUsageExpression : TransientExpression {
    public required string ServiceName { get; set; }
    public required Expression AccessExpression { get; set; }
    
    public override AstNode DeepClone() => throw new NotImplementedException();
    
    public override void Accept(IAstVisitor v) {
        if (v is IInternalAstVisitor internalVisitor) {
            internalVisitor.VisitTransientServiceUsageExpression(this);
        }

        throw new Exception("Transient service must be lowered before visiting.");
    }

    public override T Accept<T>(IAstVisitor<T> v) {
        if (v is IInternalAstVisitor<T> internalVisitor) {
            return internalVisitor.VisitTransientServiceUsageExpression(this);
        }

        throw new Exception("Transient service must be lowered before visiting.");
    }
}