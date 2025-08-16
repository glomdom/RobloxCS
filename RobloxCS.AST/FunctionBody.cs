using RobloxCS.AST.Generics;
using RobloxCS.AST.Types;

namespace RobloxCS.AST;

public sealed class FunctionBody : AstNode {
    public List<GenericDeclaration>? Generics { get; set; }
    public required List<Parameter> Parameters { get; set; }
    public required List<TypeInfo> TypeSpecifiers { get; set; }
    public required TypeInfo ReturnType { get; set; }
    public required Block Body { get; set; }
    
    public override FunctionBody DeepClone() => throw new NotImplementedException();
}