using RobloxCS.AST.Functions;

namespace RobloxCS.AST.Statements;

public sealed class FunctionDeclarationStatement : Statement {
    public required FunctionName Name { get; set; }
    public required FunctionBody Body { get; set; }

    public override FunctionDeclarationStatement DeepClone() => new() { Name = Name, Body = Body.DeepClone() };

    public override void Accept(IAstVisitor v) => v.VisitFunctionDeclaration(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitFunctionDeclaration(this);

    public override IEnumerable<AstNode> Children() {
        yield return Name;
        yield return Body;
    }
}