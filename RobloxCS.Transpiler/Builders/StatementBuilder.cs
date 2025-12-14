using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Transient;
using RobloxCS.Transpiler.Helpers;

namespace RobloxCS.Transpiler.Builders;

/// <summary>
/// Builds statements. Statement result may be a <see cref="TransientStatement"/>.
/// </summary>
public static class StatementBuilder {
    public static Statement Build(StatementSyntax stmt, TranspilationContext ctx) {
        return stmt switch {
            ExpressionStatementSyntax exprStmtSyntax => BuildFromExprStmt(exprStmtSyntax, ctx),
            LocalDeclarationStatementSyntax localDeclStmtSyntax => BuildFromLocalDeclStmt(localDeclStmtSyntax, ctx),
            BlockSyntax blockSyntax => BuildFromBlock(blockSyntax, ctx),
            IfStatementSyntax ifStatementSyntax => BuildFromIfStmt(ifStatementSyntax, ctx),
            WhileStatementSyntax whileStatementSyntax => BuildFromWhileStmt(whileStatementSyntax, ctx),
            ForStatementSyntax forStatementSyntax => BuildFromForStmt(forStatementSyntax, ctx),
            ReturnStatementSyntax returnStatementSyntax => BuildFromReturnStmt(returnStatementSyntax, ctx),
            DoStatementSyntax doStatementSyntax => BuildFromDoStmt(doStatementSyntax, ctx),
            ContinueStatementSyntax => BuildFromContinueStmt(),
            BreakStatementSyntax => BuildFromBreakStmt(),

            _ => throw new NotSupportedException($"Unsupported statement: {stmt.Kind()}"),
        };
    }

    private static BreakStatement BuildFromBreakStmt() => new();
    private static ContinueStatement BuildFromContinueStmt() => new();

    private static RepeatStatement BuildFromDoStmt(DoStatementSyntax syntax, TranspilationContext ctx) {
        var cond = ExpressionBuilder.BuildFromSyntax(syntax.Condition, ctx);
        var block = BlockBuilder.BuildFromStatement(syntax.Statement, ctx);

        var parenStmt = ExpressionHelpers.ParenthesisFromInner(cond);
        var reversedStmt = UnaryOperatorExpression.Reversed(parenStmt);
        var repeatUntilStmt = new RepeatStatement { Block = block, Until = reversedStmt };

        return repeatUntilStmt;
    }

    // TODO: Handle default initializers
    private static LocalAssignmentStatement BuildFromVarDeclaratorSyntax(VariableDeclaratorSyntax syntax, TranspilationContext ctx) {
        Expression? initExpr = null;
        if (syntax.Initializer is not null) {
            initExpr = ExpressionBuilder.BuildFromSyntax(syntax.Initializer.Value, ctx);
        }

        var symbol = ctx.Semantics.GetDeclaredSymbol(syntax);
        var type = ((ILocalSymbol)symbol!).Type;

        return StatementHelpers.SingleTypedLocalAssignment(syntax.Identifier.ValueText, initExpr!, SyntaxUtilities.BasicFromSymbol(type));
    }

    private static ReturnStatement BuildFromReturnStmt(ReturnStatementSyntax syntax, TranspilationContext ctx) {
        var returnStmt = ReturnStatement.Empty();

        if (syntax.Expression is not { } expr) return returnStmt;

        var exprResult = ExpressionBuilder.BuildFromSyntax(expr, ctx);
        returnStmt.Returns = [exprResult];

        return returnStmt;
    }

    private static TransientForLoop BuildFromForStmt(ForStatementSyntax syntax, TranspilationContext ctx) {
        var inits = new List<Statement>();

        inits.AddRange(
            syntax.Declaration?.Variables
                .Select(stx => BuildFromVarDeclaratorSyntax(stx, ctx)) ??
            syntax.Initializers
                .Select(e => BuildFromExprSyntax(e, ctx))
        );

        var transient = new TransientForLoop {
            Initializers = inits,
            Condition = syntax.Condition is not null ? ExpressionBuilder.BuildFromSyntax(syntax.Condition, ctx) : null,
            Incrementors = syntax.Incrementors.Select(expr => BuildFromExprSyntax(expr, ctx)).ToList(),
            Body = BlockBuilder.BuildFromStatement(syntax.Statement, ctx),
        };

        return transient;
    }

    private static Statement BuildFromExprSyntax(ExpressionSyntax syntax, TranspilationContext ctx) {
        switch (syntax) {
            case PostfixUnaryExpressionSyntax postExpr: {
                var tOperand = ExpressionBuilder.BuildFromSyntax(postExpr.Operand, ctx);
                var tOp = SyntaxUtilities.SyntaxTokenToCompoundOp(postExpr.OperatorToken);
                var assignment = new CompoundAssignmentStatement {
                    Left = tOperand,
                    Operator = tOp,
                    Right = new VarExpression { Expression = NumberExpression.From(1) },
                };

                return assignment;
            }

            case AssignmentExpressionSyntax assignExpr: {
                var assignment = BuildFromAssignmentExprSyntax(assignExpr, ctx);

                return assignment;
            }
        }

        throw new NotSupportedException($"{syntax.Kind()} is not supported.");
    }

    private static WhileStatement BuildFromWhileStmt(WhileStatementSyntax syntax, TranspilationContext ctx) {
        var condition = ExpressionBuilder.BuildFromSyntax(syntax.Condition, ctx);
        var stmt = Build(syntax.Statement, ctx);

        var block = BlockHelpers.From(stmt);
        var whileStmt = new WhileStatement { Condition = condition, Block = block };

        return whileStmt;
    }

    private static Statement BuildFromIfStmt(IfStatementSyntax syntax, TranspilationContext ctx) {
        var condition = ExpressionBuilder.BuildFromSyntax(syntax.Condition, ctx);
        var stmt = Build(syntax.Statement, ctx);
        var block = BlockHelpers.From(stmt);

        var elseIfBlocks = new Queue<ElseIfBlock>();
        Block? elseBlock = null;

        var elseClause = syntax.Else;
        while (elseClause is not null) {
            if (elseClause.Statement is IfStatementSyntax elseIfSyntax) {
                var elseIfCondition = ExpressionBuilder.BuildFromSyntax(elseIfSyntax.Condition, ctx);
                var elseIfStmt = Build(elseIfSyntax.Statement, ctx);
                var elseIfBlock = BlockHelpers.From(elseIfStmt);

                elseIfBlocks.Enqueue(new ElseIfBlock {
                    Condition = elseIfCondition,
                    Block = elseIfBlock,
                });

                elseClause = elseIfSyntax.Else;
            } else {
                elseBlock = BlockBuilder.BuildFromStatement(elseClause.Statement, ctx);

                break;
            }
        }

        var ifStmt = new IfStatement { Block = block, Condition = condition, Else = elseBlock, ElseIf = elseIfBlocks.ToList() };

        return ifStmt;
    }

    private static TransientBlock BuildFromBlock(BlockSyntax syntax, TranspilationContext ctx) {
        var body = BlockBuilder.Build(syntax, ctx);

        return new TransientBlock { Statements = body.Statements };
    }

    private static Statement BuildFromLocalDeclStmt(LocalDeclarationStatementSyntax localDeclStmtSyntax, TranspilationContext ctx) {
        var decl = localDeclStmtSyntax.Declaration;
        var vars = decl.Variables;

        var varNames = vars.Select(vds => vds.Identifier.ValueText).ToList();
        var initExprSyntaxes = vars.Where(v => v.Initializer is not null).Select(v => v.Initializer!.Value);
        var initExprs = initExprSyntaxes.Select(s => ExpressionBuilder.BuildFromSyntax(s, ctx)).ToList();
        var typeSym = ctx.Semantics.CheckedGetTypeInfo(decl.Type);

        var type = SyntaxUtilities.BasicFromSymbol(typeSym);
        var assignment = LocalAssignmentStatement.OfSingleType(varNames, initExprs, type);

        return assignment;
    }

    private static Statement BuildFromExprStmt(ExpressionStatementSyntax exprStmt, TranspilationContext ctx) {
        var expr = exprStmt.Expression;

        switch (expr) {
            case AssignmentExpressionSyntax assignExpr: {
                var assignment = BuildFromAssignmentExprSyntax(assignExpr, ctx);

                return assignment;
            }

            case InvocationExpressionSyntax invocationExpr: {
                var exprResult = ExpressionBuilder.BuildFromSyntax(invocationExpr, ctx);
                var stmt = FunctionCallStatement.FromExpression((FunctionCallExpression)exprResult);

                return stmt;
            }

            case PostfixUnaryExpressionSyntax postExpr: {
                var tOperand = ExpressionBuilder.BuildFromSyntax(postExpr.Operand, ctx);
                var tOp = SyntaxUtilities.SyntaxTokenToCompoundOp(postExpr.OperatorToken);
                var assignment = new CompoundAssignmentStatement {
                    Left = tOperand,
                    Operator = tOp,
                    Right = new VarExpression { Expression = NumberExpression.From(1) },
                };

                return assignment;
            }
        }

        throw new Exception($"Unhandled expression {expr.Kind()}: {expr}");
    }

    private static Statement BuildFromAssignmentExprSyntax(AssignmentExpressionSyntax expr, TranspilationContext ctx) {
        switch (expr.Kind()) {
            case SyntaxKind.SimpleAssignmentExpression: {
                var left = VarBuilder.BuildFromExpressionSyntax(expr.Left, ctx);
                var right = ExpressionBuilder.BuildFromSyntax(expr.Right, ctx);
                var assignment = new AssignmentStatement {
                    Vars = [left],
                    Expressions = [right],
                };

                return assignment;
            }

            case SyntaxKind.AddAssignmentExpression:
            case SyntaxKind.SubtractAssignmentExpression: {
                var left = ExpressionBuilder.BuildFromSyntax(expr.Left, ctx);
                var right = ExpressionBuilder.BuildFromSyntax(expr.Right, ctx);
                var tOp = SyntaxUtilities.SyntaxTokenToCompoundOp(expr.OperatorToken);
                var assignment = new CompoundAssignmentStatement { Left = left, Operator = tOp, Right = VarExpression.FromExpression(right) };

                return assignment;
            }
        }

        throw new NotSupportedException($"{expr.Kind()} is not supported.");
    }

    private static bool RequiresDoScope(BlockSyntax block, TranspilationContext ctx) {
        var parentKind = block.Parent?.Kind();
        var isDirectBody = parentKind is SyntaxKind.IfStatement or SyntaxKind.ElseClause or SyntaxKind.ForStatement or SyntaxKind.WhileStatement or SyntaxKind.DoStatement;

        if (!isDirectBody) return true;

        foreach (var stmt in block.Statements) {
            if (stmt is not LocalDeclarationStatementSyntax local) continue;

            if (local.Declaration.Variables.Select(v => ctx.Semantics.GetDeclaredSymbol(v)).OfType<ISymbol>()
                .Any(symbol => symbol.ContainingSymbol.Kind == SymbolKind.Method)) {
                return true;
            }
        }

        return false;
    }
}