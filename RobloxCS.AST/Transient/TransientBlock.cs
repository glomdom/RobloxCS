using RobloxCS.AST.Statements;

namespace RobloxCS.AST.Transient;

/// <summary>
/// A block containing statements. Can be standalone or inside a container.
/// </summary>
public sealed class TransientBlock : TransientStatement {
    public required List<Statement> Statements { get; set; } = [];
    
    public override AstNode DeepClone() => throw new NotImplementedException();
}