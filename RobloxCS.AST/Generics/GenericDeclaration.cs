namespace RobloxCS.AST.Generics;

public sealed class GenericDeclaration : AstNode {
    public required List<GenericDeclarationParameter> Parameters { get; set; }

    public override GenericDeclaration DeepClone() => new() {
        Parameters = Parameters.Select(p => (GenericDeclarationParameter)p.DeepClone()).ToList()
    };
}