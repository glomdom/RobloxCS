using RobloxCS.AST.Generics;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public sealed class FunctionBody : AstNode {
    public List<GenericDeclaration>? Generics { get; set; }
    public required List<Parameter> Parameters { get; set; }
    public required List<TypeInfo> TypeSpecifiers { get; set; }
    public required TypeInfo ReturnType { get; set; }
    public required Block Body { get; set; }

    public override FunctionBody DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitFunctionBody(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitFunctionBody(this);

    public override IEnumerable<AstNode> Children() {
        if (Generics is not null) {
            foreach (var generic in Generics) yield return generic;
        }

        foreach (var parameter in Parameters) yield return parameter;
        foreach (var ts in TypeSpecifiers) yield return ts;

        yield return ReturnType;
        yield return Body;
    }
}