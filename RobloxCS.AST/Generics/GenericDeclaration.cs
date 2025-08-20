﻿namespace RobloxCS.AST.Generics;

public sealed class GenericDeclaration : AstNode {
    public required List<GenericDeclarationParameter> Parameters { get; set; }

    public override GenericDeclaration DeepClone() => new() {
        Parameters = Parameters.Select(p => (GenericDeclarationParameter)p.DeepClone()).ToList(),
    };

    public override void Accept(IAstVisitor v) => v.VisitGenericDeclaration(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitGenericDeclaration(this);

    public override IEnumerable<AstNode> Children() {
        return Parameters;
    }
}