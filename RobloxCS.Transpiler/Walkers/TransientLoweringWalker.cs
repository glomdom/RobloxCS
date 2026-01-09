using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
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
        FlattenAndAdd(node.Body.Statements, loopBlock);

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

    public AstNode VisitTransientBlock(TransientBlock node) {
        var newBlock = BlockHelpers.Empty();
        FlattenAndAdd(node.Statements, newBlock);

        return StatementHelpers.DoFromBlock(newBlock);
    }

    public override AstNode VisitIfStatement(IfStatement node) {
        var cond = (Expression)Visit(node.Condition);
        var final = BlockHelpers.Empty();

        FlattenAndAdd(node.Block.Statements, final);

        var elseBlock = (Block?)node.Else?.Accept(this);
        var elseifBlocks = node.ElseIf?.Select(ei => (ElseIfBlock)ei.Accept(this)).ToList();

        return new IfStatement {
            Condition = cond,
            Block = final,
            Else = elseBlock,
            ElseIf = elseifBlocks,
        };
    }

    public override AstNode VisitWhileStatement(WhileStatement node) {
        _incrementorStack.Push(null);

        var cond = (Expression)Visit(node.Condition);
        var final = BlockHelpers.Empty();
        FlattenAndAdd(node.Block.Statements, final);

        _incrementorStack.Pop();

        return new WhileStatement {
            Condition = cond,
            Block = final,
        };
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
        if (currentIncrementors is null || currentIncrementors.Count == 0) return node;

        var block = BlockHelpers.Empty();
        foreach (var inc in currentIncrementors) {
            block.AddStatement((Statement)inc.DeepClone());
        }

        block.AddStatement(new ContinueStatement());

        return block;
    }

    public override AstNode VisitBlock(Block node) {
        var newBlock = BlockHelpers.Empty();
        FlattenAndAdd(node.Statements, newBlock);

        return newBlock;
    }

    private void FlattenAndAdd(IList<Statement> statements, Block targetBlock) {
        foreach (var stmt in statements) {
            if (stmt is TransientBlock transient) {
                FlattenAndAdd(transient.Statements, targetBlock);
            } else {
                var visited = stmt.Accept(this);
                switch (visited) {
                    case Block b: targetBlock.AddBlock(b); break;
                    case Statement s: targetBlock.AddStatement(s); break;
                }
            }
        }
    }
}