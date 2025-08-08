using System.Text;
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
            Types = [type]
        };
    }

    public override LocalAssignment DeepClone() => new() {
        Names = Names.Select(n => (SymbolExpression)n.DeepClone()).ToList(),
        Expressions = Expressions.Select(e => (Expression)e.DeepClone()).ToList(),
        Types = Types.Select(t => (TypeInfo)t.DeepClone()).ToList(),
    };
}