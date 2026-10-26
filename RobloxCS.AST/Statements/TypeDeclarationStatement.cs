using RobloxCS.AST.Generics;
using RobloxCS.AST.Types;

namespace RobloxCS.AST.Statements;

public sealed class TypeDeclarationStatement : Statement {
    public required string Name { get; set; }
    public List<GenericDeclaration>? Declarations { get; set; }
    public required TypeInfo DeclareAs { get; set; }
    public bool Exported { get; set; } = false;

    public static TypeDeclarationStatement EmptyTable(string name) {
        return new TypeDeclarationStatement {
            Name = name,
            DeclareAs = TableTypeInfo.Empty(),
        };
    }

    public override TypeDeclarationStatement DeepClone() => new() {
        Name = Name,
        Declarations = Declarations?.Select(decl => decl.DeepClone()).ToList(),
        DeclareAs = (TypeInfo)DeclareAs.DeepClone(),
        Exported = Exported,
    };

    public override void Accept(IAstVisitor v) => v.VisitTypeDeclaration(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTypeDeclaration(this);

    public override IEnumerable<AstNode> Children() {
        if (Declarations is not null) {
            foreach (var decl in Declarations) {
                yield return decl;
            }
        }

        yield return DeclareAs;
    }
}