using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Functions;

public sealed class FunctionArgs : AstNode {
    public required List<Expression> Arguments { get; set; }

    public static FunctionArgs Empty() {
        return new FunctionArgs { Arguments = [] };
    }

    public static FunctionArgs FromExpressions(List<Expression> exprs) => new() { Arguments = exprs };

    public override FunctionArgs DeepClone() => new() { Arguments = Arguments.Select(arg => (Expression)arg.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitFunctionArgs(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitFunctionArgs(this);

    public override IEnumerable<AstNode> Children() => Arguments;
}