using RobloxCS.AST.Functions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.AST.Expressions;

public class FunctionCallExpression : Expression {
    public required Prefix Prefix { get; set; }
    public required List<Suffix> Suffixes { get; set; }

    public static FunctionCallExpression Basic(string name, params Expression[] args) {
        return new FunctionCallExpression {
            Prefix = new NamePrefix { Name = name },
            Suffixes = [
                new AnonymousCall {
                    Arguments = new FunctionArgs {
                        Arguments = args.ToList(),
                    },
                },
            ],
        };
    }

    public override FunctionCallExpression DeepClone() => new() { Prefix = (Prefix)Prefix.DeepClone(), Suffixes = Suffixes.Select(s => (Suffix)s.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitFunctionCall(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitFunctionCall(this);

    public override IEnumerable<AstNode> Children() {
        yield return Prefix;

        foreach (var suffix in Suffixes) yield return suffix;
    }
}