using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Transient;
using RobloxCS.Transpiler.Helpers;
using Serilog;

namespace RobloxCS.Transpiler.Walkers;

public sealed class TransientLoweringWalker : AstRewriter, IInternalAstVisitor<AstNode> {
    public AstNode VisitTransientForLoop(TransientForLoop node) {
        Log.Debug("Lowering transient for loop with {InitializerCount} initializers", node.Initializers.Count);

        var inner = BlockHelpers.Empty();

        foreach (var init in node.Initializers) {
            inner.AddStatement(init);
        }

        var loopBlock = BlockHelpers.Empty();
        var whileStmt = new WhileStatement {
            Condition = BooleanExpression.True(),
            Block = loopBlock,
        };

        // explicit condition
        {
            var explicitCondition = new IfStatement {
                Condition = UnaryOperatorExpression.Reversed(node.Condition!),
                Block = BlockHelpers.From(new BreakStatement()),
            };

            loopBlock.AddStatement(explicitCondition);
        }

        // body
        {
            foreach (var lowered in node.Body.Statements.Select(stmt => stmt.Accept(this))) {
                if (lowered is Statement loweredStmt) {
                    loopBlock.AddStatement(loweredStmt);
                }
            }
        }

        // incrementors
        {
            foreach (var incr in node.Incrementors) {
                loopBlock.AddStatement(incr);
            }
        }

        inner.AddStatement(whileStmt);

        return StatementHelpers.DoFromBlock(inner);
    }
}