using RobloxCS.AST.Expressions;
using RobloxCS.AST.Types;

namespace RobloxCS.AST.Statements;

public sealed class LocalAssignment : Statement {
    public required List<SymbolExpression> Names { get; set; }
    public required List<Expression> Expressions { get; set; }
    public required List<TypeInfo> Types { get; set; }

    public static LocalAssignment Naked(string name, TypeInfo type) {
        return new LocalAssignment {
            Names = [SymbolExpression.FromString(name)],
            Expressions = [],
            Types = [type],
        };
    }

    public static LocalAssignment OfSingleType(List<string> names, List<Expression> expressions, TypeInfo type) {
        return new LocalAssignment {
            Names = names.Select(SymbolExpression.FromString).ToList(),
            Expressions = expressions,
            Types = Enumerable.Repeat(type, names.Count).ToList(),
        };
    }

    public override LocalAssignment DeepClone() => new() {
        Names = Names.Select(n => n.DeepClone()).ToList(),
        Expressions = Expressions.Select(e => (Expression)e.DeepClone()).ToList(),
        Types = Types.Select(t => (TypeInfo)t.DeepClone()).ToList(),
    };

    public override void Accept(IAstVisitor v) => v.VisitLocalAssignment(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitLocalAssignment(this);

    public override IEnumerable<AstNode> Children() {
        foreach (var n in Names) yield return n;
        foreach (var e in Expressions) yield return e;
        foreach (var t in Types) yield return t;
    }
}