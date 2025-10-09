using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.AST.Statements;

public class FunctionCallStatement : Statement {
    public required Prefix Prefix;
    public required List<Suffix> Suffixes;

    public static FunctionCallStatement FromExpression(FunctionCallExpression expr) => new() { Prefix = expr.Prefix, Suffixes = expr.Suffixes };

    public override FunctionCallStatement DeepClone() => new() { Prefix = (Prefix)Prefix.DeepClone(), Suffixes = Suffixes.Select(s => (Suffix)s.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitFunctionCallStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.DefaultVisit(this);
}