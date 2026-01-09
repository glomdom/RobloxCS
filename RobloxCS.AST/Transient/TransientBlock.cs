using RobloxCS.AST.Statements;

namespace RobloxCS.AST.Transient;

/// <summary>
/// A block containing statements. Can be standalone or inside a container.
/// </summary>
public sealed class TransientBlock : TransientStatement {
    public required List<Statement> Statements { get; set; } = [];
    
    public override AstNode DeepClone() => throw new NotImplementedException();
    
    public override void Accept(IAstVisitor v) {
        if (v is IInternalAstVisitor internalVisitor) {
            internalVisitor.VisitTransientBlock(this);
        }

        throw new Exception("Transient block be lowered before lowering.");
    }

    public override T Accept<T>(IAstVisitor<T> v) {
        if (v is IInternalAstVisitor<T> internalVisitor) {
            return internalVisitor.VisitTransientBlock(this);
        }

        throw new Exception("Transient block must be lowered before lowering.");
    }
}