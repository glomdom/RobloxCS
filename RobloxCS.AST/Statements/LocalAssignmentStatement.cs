using RobloxCS.AST.Expressions;
using RobloxCS.AST.Types;

namespace RobloxCS.AST.Statements;

public sealed class LocalAssignmentStatement : Statement {
    public required List<SymbolExpression> Names { get; set; }
    public required List<Expression> Expressions { get; set; }
    public required List<TypeInfo> Types { get; set; }

    public static LocalAssignmentStatement Empty() => new() { Expressions = [], Names = [], Types = [] };

    public static LocalAssignmentStatement Single(string name, Expression expr, TypeInfo type) {
        return new LocalAssignmentStatement {
            Names = [SymbolExpression.FromString(name)],
            Expressions = [expr],
            Types = [type],
        };
    }

    public static LocalAssignmentStatement Naked(string name, TypeInfo type) {
        return new LocalAssignmentStatement {
            Names = [SymbolExpression.FromString(name)],
            Expressions = [],
            Types = [type],
        };
    }

    public static LocalAssignmentStatement OfSingleType(List<string> names, List<Expression> expressions, TypeInfo type) {
        return new LocalAssignmentStatement {
            Names = names.Select(SymbolExpression.FromString).ToList(),
            Expressions = expressions,
            Types = Enumerable.Repeat(type, names.Count).ToList(),
        };
    }

    public override LocalAssignmentStatement DeepClone() => new() {
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