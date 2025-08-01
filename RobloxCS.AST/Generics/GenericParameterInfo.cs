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
}

