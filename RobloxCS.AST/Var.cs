using RobloxCS.AST.Expressions;

namespace RobloxCS.AST;

public abstract class Var : AstNode;

public sealed class VarExpression : Var {
    public required Expression Expression { get; set; }
    
    public static VarExpression FromExpression(Expression expr) => new() { Expression = expr };
}

public sealed class VarName : Var {
    public required string Name { get; set; }

    public static VarName FromSymbol(SymbolExpression sym) => new() { Name = sym.Value };
    public static VarName FromString(string str) => new() { Name = str };
}