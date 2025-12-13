using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Transient;
using RobloxCS.Transpiler.Helpers;
using Serilog;

namespace RobloxCS.Transpiler.Walkers;

public sealed class TransientLoweringWalker : AstRewriter, IInternalAstVisitor<AstNode> {
    private readonly Stack<List<Statement>?> _incrementorStack = new();

    public AstNode VisitTransientForLoop(TransientForLoop node) {
        Log.Debug("Lowering transient for loop with {InitializerCount} initializers, {IncrementorCount} incrementors", node.Initializers.Count, node.Incrementors.Count);

        _incrementorStack.Push(node.Incrementors);

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

        var doStmt = StatementHelpers.DoFromBlock(inner);

        _incrementorStack.Pop();

        return doStmt;
    }

    public override AstNode VisitWhileStatement(WhileStatement node) {
        _incrementorStack.Push(null);

        var stmt = base.VisitWhileStatement(node);

        _incrementorStack.Pop();

        return stmt;
    }

    public override AstNode VisitDoStatement(DoStatement node) {
        _incrementorStack.Push(null);

        var stmt = base.VisitDoStatement(node);

        _incrementorStack.Pop();

        return stmt;
    }

    public override AstNode VisitContinueStatement(ContinueStatement node) {
        if (_incrementorStack.Count <= 0) return node;

        var currentIncrementors = _incrementorStack.Peek();
        if (currentIncrementors is null || currentIncrementors.Count != 0) return node;

        var block = BlockHelpers.Empty();
        foreach (var inc in currentIncrementors) {
            block.AddStatement((Statement)inc.DeepClone());
        }

        block.AddStatement(new ContinueStatement());

        return block;
    }
}