using RobloxCS.AST.Generics;

namespace RobloxCS.AST.Types;

public sealed class TypeDeclaration : AstNode {
    public required string Name { get; set; }
    public List<GenericDeclaration>? Declarations { get; set; }
    public required TypeInfo DeclareAs { get; set; }
}