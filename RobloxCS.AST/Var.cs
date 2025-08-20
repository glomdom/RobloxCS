using RobloxCS.AST.Expressions;

namespace RobloxCS.AST;

public abstract class Var : AstNode;

public sealed class VarExpression : Var {
    public required Expression Expression { get; set; }

    public static VarExpression FromExpression(Expression expr) => new() { Expression = expr };

    public override VarExpression DeepClone() => new() { Expression = (Expression)Expression.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitVarExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitVarExpression(this);

    public override IEnumerable<AstNode> Children() {
        yield return Expression;
    }
}

public sealed class VarName : Var {
    public required string Name { get; set; }

    public static VarName FromSymbol(SymbolExpression sym) => new() { Name = sym.Value };
    public static VarName FromString(string str) => new() { Name = str };

    public override VarName DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.VisitVarName(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitVarName(this);
}