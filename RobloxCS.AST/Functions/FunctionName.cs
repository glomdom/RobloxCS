using System.Text;

namespace RobloxCS.AST.Functions;

public sealed class FunctionName : AstNode {
    public required List<string> Names { get; set; }
    public string? ColonName { get; set; }

    public override FunctionName DeepClone() => new() { Names = Names.Select(n => n).ToList(), ColonName = ColonName };
    public override void Accept(IAstVisitor v) => v.VisitFunctionName(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitFunctionName(this);

    public override string ToString() {
        return string.Join('.', Names) + (ColonName is not null ? $":{ColonName}" : string.Empty);
    }
}