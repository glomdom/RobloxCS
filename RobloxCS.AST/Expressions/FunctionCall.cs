using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;

namespace RobloxCS.AST.Expressions;

public class FunctionCall : Expression {
    public required Prefix Prefix { get; set; }
    public required List<Suffix> Suffixes { get; set; }

    public static FunctionCall Basic(string name, params Expression[] args) {
        return new FunctionCall {
            Prefix = new NamePrefix { Name = name },
            Suffixes = [
                new AnonymousCall {
                    Arguments = new FunctionArgs {
                        Arguments = args.ToList()
                    }
                }
            ]
        };
    }

    public override FunctionCall DeepClone() => new() { Prefix = (Prefix)Prefix.DeepClone(), Suffixes = Suffixes.Select(s => (Suffix)s.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.Visit(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.Visit(this);
}