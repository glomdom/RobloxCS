using RobloxCS.AST.Types;

namespace RobloxCS.AST.Generics;

public sealed class GenericDeclarationParameter : AstNode {
    public required GenericParameterInfo Parameter { get; set; }
    public TypeInfo? Default { get; set; }

    public override GenericDeclarationParameter DeepClone() => new() {
        Parameter = (GenericParameterInfo)Parameter.DeepClone(), Default = (TypeInfo?)Default?.DeepClone(),
    };

    public override void Accept(IAstVisitor v) => v.VisitGenericDeclarationParameter(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitGenericDeclarationParameter(this);

    public override IEnumerable<AstNode> Children() {
        yield return Parameter;

        if (Default is not null) {
            yield return Default;
        }
    }
}