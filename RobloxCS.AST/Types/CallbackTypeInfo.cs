using RobloxCS.AST.Generics;

namespace RobloxCS.AST.Types;

public sealed class CallbackTypeInfo : TypeInfo {
    public GenericDeclaration? Generics;
    public required List<TypeArgument> Arguments;
    public required TypeInfo ReturnType;

    public override CallbackTypeInfo DeepClone() => new() {
        Generics = (GenericDeclaration?)Generics?.DeepClone(),
        Arguments = Arguments.Select(arg => arg.DeepClone()).ToList(),
        ReturnType = (TypeInfo)ReturnType.DeepClone()
    };
    
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}