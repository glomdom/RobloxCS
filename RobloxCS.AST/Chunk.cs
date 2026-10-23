namespace RobloxCS.AST;

public sealed class Chunk : AstNode {
    public required Block Block { get; set; }
    public required TypeCheckMode Mode { get; set; }

    public override Chunk DeepClone() => new() { Block = Block.DeepClone(), Mode = Mode };
    public override void Accept(IAstVisitor v) => v.VisitChunk(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitChunk(this);

    public override IEnumerable<AstNode> Children() {
        yield return Block;
    }
}