using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.Transpiler.Scoping;

namespace RobloxCS.Transpiler.Builders;

public class StatementBuilder {
    public static Statement Build(StatementSyntax stmt, TranspilationContext ctx) {
        return stmt switch {
            ExpressionStatementSyntax exprStmtSyntax => BuildFromExprStmt(exprStmtSyntax, ctx),
            LocalDeclarationStatementSyntax localDeclStmtSyntax => BuildFromLocalDeclStmt(localDeclStmtSyntax, ctx),
            BlockSyntax blockSyntax => BuildFromBlockStmt(blockSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported statement: {stmt.Kind()}"),
        };
    }

    private static Statement BuildFromBlockStmt(BlockSyntax syntax, TranspilationContext ctx) {
        var block = Block.Empty();

        ctx.PushScope();
        var statements = syntax.Statements.Select(statement => Build(statement, ctx)).ToList();
        ctx.PopScope();

        block.Statements = statements;

        return DoStatement.FromBlock(block);
    }

    private static Statement BuildFromLocalDeclStmt(LocalDeclarationStatementSyntax localDeclStmtSyntax, TranspilationContext ctx) {
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