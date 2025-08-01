using RobloxCS.AST.Types;

namespace RobloxCS.AST.Generics;

public sealed class GenericDeclarationParameter : AstNode {
    public required GenericParameterInfo Parameter { get; set; }
    public TypeInfo? Default { get; set; }
}