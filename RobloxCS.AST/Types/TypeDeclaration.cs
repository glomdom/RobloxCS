using RobloxCS.AST.Generics;

namespace RobloxCS.AST.Types;

public sealed class TypeDeclaration : AstNode {
    public required string Name { get; set; }
    public List<GenericDeclaration>? Declarations { get; set; }
    public required TypeInfo DeclareAs { get; set; }

    public static TypeDeclaration EmptyTable(string name) {
        return new TypeDeclaration {
            Name = name,
            DeclareAs = TableTypeInfo.Empty(),
        };
    }

    public override TypeDeclaration DeepClone() => new() {
        Name = Name,
        Declarations = Declarations?.Select(decl => (GenericDeclaration)decl.DeepClone()).ToList(),
        DeclareAs = (TypeInfo)DeclareAs.DeepClone()
    };
}