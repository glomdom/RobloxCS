using RobloxCS.AST.Generics;

namespace RobloxCS.AST.Types;

public sealed class CallbackTypeInfo : TypeInfo {
    public GenericDeclaration? Generics;
    public required List<TypeArgument> Arguments;
    public required TypeInfo ReturnType;
}