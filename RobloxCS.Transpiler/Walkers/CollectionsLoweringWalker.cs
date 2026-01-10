using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;
using RobloxCS.Transpiler.Semantics;
using Serilog;

namespace RobloxCS.Transpiler.Walkers;

public class CollectionsLoweringWalker : SemanticRewriter {
    public CollectionsLoweringWalker(GlobalRegistry registry) : base(registry) { }

    public override AstNode VisitVarExpression(VarExpression node) {
        if (node.Suffixes.Count == 0) return base.VisitVarExpression(node);

        if (node.Suffixes.Last() is Dot { Name.Value: "Count" }) {
            var lhs = node.DeepClone();
            lhs.Suffixes.RemoveAt(lhs.Suffixes.Count - 1);

            var lhsType = ResolveType(lhs);

            if (lhsType is ArrayTypeInfo) {
                var visited = Visit(lhs);

                return new UnaryOperatorExpression {
                    Expression = (Expression)visited,
                    UnOp = UnOp.Hash,
                };
            }
        }

        return node;
    }
}