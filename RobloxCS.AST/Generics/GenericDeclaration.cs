namespace RobloxCS.AST.Generics;

public sealed class GenericDeclaration : AstNode {
    public required List<GenericDeclarationParameter> Parameters { get; set; }
}