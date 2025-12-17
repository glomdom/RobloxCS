using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.AST;

public abstract class Var : Expression;

public sealed class VarExpression : Var {
    public required Prefix Prefix { get; set; }
    public List<Suffix> Suffixes { get; set; } = [];

    public Var? ParentVar => Parent as Var;

    public override VarExpression DeepClone() => new() { Prefix = (Prefix)Prefix.DeepClone(), Suffixes = Suffixes.Select(s => (Suffix)s.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitVarExpression(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitVarExpression(this);

    public override IEnumerable<AstNode> Children() {
        yield return Prefix;

        foreach (var suffix in Suffixes) {
            yield return suffix;
        }
    }
}

public sealed class VarName : Var {
    public required string Name { get; set; }

    public Var? ParentVar => Parent as Var;

    public static VarName FromSymbol(SymbolExpression sym) => new() { Name = sym.Value };
    public static VarName FromString(string str) => new() { Name = str };

    public override VarName DeepClone() => new() { Name = Name };
    public override void Accept(IAstVisitor v) => v.VisitVarName(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitVarName(this);
}