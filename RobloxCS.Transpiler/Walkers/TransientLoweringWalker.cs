using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Transient;
using RobloxCS.Transpiler.Helpers;

namespace RobloxCS.Transpiler.Walkers;

public sealed class TransientLoweringWalker : AstRewriter, IInternalAstVisitor<AstNode> {
    public AstNode VisitTransientForLoop(TransientForLoop node) {
        return new WhileStatement {
            Block = BlockHelpers.Empty(),
            Condition = ExpressionHelpers.SymbolFromString("123"),
        };
    }
}