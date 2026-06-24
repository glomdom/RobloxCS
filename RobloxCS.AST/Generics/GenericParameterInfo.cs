using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Generics;

public abstract class GenericParameterInfo : AstNode;

/// <summary>
/// A name: <c>T</c>
/// </summary>
public sealed class NameGenericParameter : GenericParameterInfo {
    public required string Name { get; set; }

    public static NameGenericParameter FromString(string name) {
        return new NameGenericParameter { Name = name };
    }

    public static NameGenericParameter FromSymbol(SymbolExpression expr) {
        return new NameGenericParameter { Name = expr.Value };
    }

    public override NameGenericParameter DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.VisitNameGenericParameter(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitNameGenericParameter(this);
}

/// <summary>
/// A variadic type pack: <c>T...</c>
/// </summary>
public sealed class VariadicGenericParameter : GenericParameterInfo {
    public required string Name { get; set; }

    public static VariadicGenericParameter FromString(string name) {
        return new VariadicGenericParameter { Name = name };
    }

    public static VariadicGenericParameter FromSymbol(SymbolExpression expr) {
        return new VariadicGenericParameter { Name = expr.Value };
    }
    
    public override VariadicGenericParameter DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.VisitVariadicGenericParameter(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitVariadicGenericParameter(this);
}