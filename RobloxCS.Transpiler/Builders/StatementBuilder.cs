using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.Transpiler.Scoping;
using Serilog;

namespace RobloxCS.Transpiler.Builders;

public class StatementBuilder {
    public static Statement Build(StatementSyntax stmt, TranspilationContext ctx) {
        return stmt switch {
            ExpressionStatementSyntax exprStmtSyntax => BuildFromExprStmt(exprStmtSyntax, ctx),
            LocalDeclarationStatementSyntax localDeclStmtSyntax => BuildFromLocalDeclStmt(localDeclStmtSyntax, ctx),
            BlockSyntax blockSyntax => BuildFromBlockStmt(blockSyntax, ctx),
            IfStatementSyntax ifStatementSyntax => BuildFromIfStmt(ifStatementSyntax, ctx),
            WhileStatementSyntax whileStatementSyntax => BuildFromWhileStmt(whileStatementSyntax, ctx),
            ForStatementSyntax forStatementSyntax => BuildFromForStmt(forStatementSyntax, ctx),
            ReturnStatementSyntax returnStatementSyntax => BuildFromReturnStmt(returnStatementSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported statement: {stmt.Kind()}"),
        };
    }

    private static ReturnStatement BuildFromReturnStmt(ReturnStatementSyntax syntax, TranspilationContext ctx) {
        var stmt = new ReturnStatement { Returns = [] };

        if (syntax.Expression is { } expr) {
            stmt.Returns = [ExpressionBuilder.BuildFromSyntax(expr, ctx)];
        }

        return stmt;
    }

    // TODO: Do not desugar unless we can prove the following:
    //       1. initializer is a constant number
    //       2. the condition is a simple `< constant`
    //       3. increment is `i++` or `i += constant`
    private static Statement BuildFromForStmt(ForStatementSyntax syntax, TranspilationContext ctx) {
        var block = Block.Empty();
        var doStmt = new DoStatement { Block = block };

        ctx.PushScope();

        var loopVarBinding = BuildLoopVarAssignment(syntax, ctx);
        block.AddStatement(loopVarBinding);

        var whileBlock = Block.Empty();
        var whileLoop = new WhileStatement { Block = whileBlock, Condition = BooleanExpression.True() };

        ctx.PushScope();

        if (syntax.Condition is null) {
            throw new NotSupportedException("While loops without conditions are not supported.");
        }

        var rawCond = ExpressionBuilder.BuildFromSyntax(syntax.Condition, ctx);
        var reverseCond = UnaryOperatorExpression.Reversed(rawCond);
        var ifStmt = new IfStatement { Condition = reverseCond, Block = Block.From(new BreakStatement()) };
        whileBlock.AddStatement(ifStmt);

        var stmt = Build(syntax.Statement, ctx);
        var whileBlockStmt = stmt is DoStatement whileDoStmt ? whileDoStmt.Block : Block.From(stmt);
        whileBlock.AddBlock(whileBlockStmt);

        var lastStmt = syntax.Incrementors.Select(expr => BuildFromExprSyntax(expr, ctx)).ToList();
        whileBlock.AddStatements(lastStmt);

        ctx.PopScope();

        block.AddStatement(whileLoop);
        ctx.PopScope();

        return doStmt;
    }

    private static Statement BuildFromExprSyntax(ExpressionSyntax syntax, TranspilationContext ctx) {
        Log.Verbose("Building statement from expression syntax of kind {SyntaxKind}", syntax.Kind());

        switch (syntax) {
            case PostfixUnaryExpressionSyntax postExpr: {
                var tOperand = ExpressionBuilder.BuildFromSyntax(postExpr.Operand, ctx);
                var tOp = SyntaxUtilities.SyntaxTokenToCompoundOp(postExpr.OperatorToken);

                return new CompoundAssignmentStatement { Left = tOperand, Operator = tOp, Right = new VarExpression { Expression = NumberExpression.From(1) } };
            }

            case AssignmentExpressionSyntax assignExpr: {
                var assignment = BuildFromAssignmentExprSyntax(assignExpr, ctx);

                return assignment;
            }
        }

        throw new NotSupportedException($"{syntax.Kind()} is not supported.");
    }

    private static LocalAssignmentStatement BuildLoopVarAssignment(ForStatementSyntax syntax, TranspilationContext ctx) {
        if (syntax.Declaration is null) {
            throw new NotImplementedException("For loops without variable declarations are not supported yet.");
        }

        var vars = syntax.Declaration.Variables;
        var names = vars.Select(vds => vds.Identifier.ValueText).Select(SymbolExpression.FromString).ToList();
        var initializers = vars
            .Where(v => v.Initializer is not null)
            .Select(v => v.Initializer!.Value)
            .Select(es => ExpressionBuilder.BuildFromSyntax(es, ctx))
            .ToList();

        var binding = new LocalAssignmentStatement { Expressions = initializers, Names = names, Types = [] };

        return binding;
    }

    private static WhileStatement BuildFromWhileStmt(WhileStatementSyntax syntax, TranspilationContext ctx) {
        var condition = ExpressionBuilder.BuildFromSyntax(syntax.Condition, ctx);
        var stmt = Build(syntax.Statement, ctx);
        var block = stmt is DoStatement doStmt ? doStmt.Block : Block.From(stmt);

        return new WhileStatement { Condition = condition, Block = block };
    }

    private static IfStatement BuildFromIfStmt(IfStatementSyntax syntax, TranspilationContext ctx) {
        var condition = ExpressionBuilder.BuildFromSyntax(syntax.Condition, ctx);
        var stmt = Build(syntax.Statement, ctx);
        var block = stmt is DoStatement doStmt ? doStmt.Block : Block.From(stmt);

        var elseIfBlocks = new Queue<ElseIfBlock>();
        Block? elseBlock = null;

        var elseClause = syntax.Else;
        while (elseClause is not null) {
            if (elseClause.Statement is IfStatementSyntax elseIfSyntax) {
                var elseIfCondition = ExpressionBuilder.BuildFromSyntax(elseIfSyntax.Condition, ctx);
                var elseIfStmt = Build(elseIfSyntax.Statement, ctx);
                var elseIfBlock = elseIfStmt is DoStatement doElseIfStmt ? doElseIfStmt.Block : Block.From(elseIfStmt);

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

        return new IfStatement { Block = block, Condition = condition, Else = elseBlock, ElseIf = elseIfBlocks.ToList() };
    }

    private static DoStatement BuildFromBlockStmt(BlockSyntax syntax, TranspilationContext ctx) {
        return DoStatement.FromBlock(BlockBuilder.Build(syntax, ctx));
    }

    private static LocalAssignmentStatement BuildFromLocalDeclStmt(LocalDeclarationStatementSyntax localDeclStmtSyntax, TranspilationContext ctx) {
        var decl = localDeclStmtSyntax.Declaration;
        var vars = decl.Variables;

        var varNames = vars.Select(vds => vds.Identifier.ValueText).ToList();
        var initExprSyntaxes = vars.Where(v => v.Initializer is not null).Select(v => v.Initializer!.Value);
        var initExprs = initExprSyntaxes.Select(s => ExpressionBuilder.BuildFromSyntax(s, ctx)).ToList();

        var typeSym = ctx.Semantics.CheckedGetTypeInfo(decl.Type);

        var type = SyntaxUtilities.BasicFromSymbol(typeSym);
        var varSymbols = varNames.Select(vn => new VariableSymbol(vn, type)).ToList();

        for (var i = 0; i < vars.Count; i++) {
            var success = ctx.CurrentScope.TryDeclare(varNames[i], varSymbols[i]);
            if (!success) {
                throw new Exception($"Failed to declare {varNames[i]} in scope.");
            }
        }

        return LocalAssignmentStatement.OfSingleType(varNames, initExprs, type);
    }

    private static Statement BuildFromExprStmt(ExpressionStatementSyntax exprStmt, TranspilationContext ctx) {
        var expr = exprStmt.Expression;

        Log.Verbose("Building statement from expression statement of kind {StatementKind}", expr.Kind());

        switch (expr) {
            case AssignmentExpressionSyntax assignExpr: {
                var assignment = BuildFromAssignmentExprSyntax(assignExpr, ctx);

                return assignment;
            }

            case PostfixUnaryExpressionSyntax postExpr: {
                var tOperand = ExpressionBuilder.BuildFromSyntax(postExpr.Operand, ctx);
                var tOp = SyntaxUtilities.SyntaxTokenToCompoundOp(postExpr.OperatorToken);

                return new CompoundAssignmentStatement { Left = tOperand, Operator = tOp, Right = new VarExpression { Expression = NumberExpression.From(1) } };
            }
        }

        throw new Exception($"Unhandled expression {expr.Kind()}");
    }

    private static Statement BuildFromAssignmentExprSyntax(AssignmentExpressionSyntax expr, TranspilationContext ctx) {
        switch (expr.Kind()) {
            case SyntaxKind.SimpleAssignmentExpression: {
                var left = VarBuilder.BuildFromExpressionSyntax(expr.Left, ctx);
                var right = ExpressionBuilder.BuildFromSyntax(expr.Right, ctx);

                return new AssignmentStatement {
                    Vars = [left],
                    Expressions = [right],
                };
            }

            case SyntaxKind.AddAssignmentExpression: {
                var left = ExpressionBuilder.BuildFromSyntax(expr.Left, ctx);
                var right = ExpressionBuilder.BuildFromSyntax(expr.Right, ctx);
                var tOp = SyntaxUtilities.SyntaxTokenToCompoundOp(expr.OperatorToken);

                return new CompoundAssignmentStatement { Left = left, Operator = tOp, Right = VarExpression.FromExpression(right) };
            }
        }

        throw new NotSupportedException($"{expr.Kind()} is not supported.");
    }
}