using RobloxCS.AST;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Suffixes;
using Serilog;

namespace RobloxCS.Transpiler.Walkers;

public class CollectionsLoweringWalker : AstRewriter, IInternalAstVisitor<AstNode> {
    public override AstNode VisitDot(Dot node) {
        Log.Verbose("Parent is {Parent} ({Node})", node.Parent, node);

        if (node.Parent is VarExpression vExp) {
            Log.Verbose("and has expression {ExpressionType}", vExp.Prefix.GetType().Name);
        }

        return node;
    }
}