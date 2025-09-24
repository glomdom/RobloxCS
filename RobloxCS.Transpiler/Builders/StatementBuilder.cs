using System.Collections;
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

            _ => throw new NotSupportedException($"Unsupported statement: {stmt.Kind()}"),
        };
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

        switch (expr) {
            case AssignmentExpressionSyntax assignExpr: {
                var left = VarBuilder.BuildFromExpressionSyntax(assignExpr.Left, ctx);
                var right = ExpressionBuilder.BuildFromSyntax(assignExpr.Right, ctx);

                return new AssignmentStatement {
                    Vars = [left],
                    Expressions = [right],
                };
            }
        }

        throw new Exception($"Unhandled expression {expr.Kind()}");
    }
}