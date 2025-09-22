using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Builders;

public class StatementBuilder {
    public static Statement Transpile(StatementSyntax stmt, TranspilationContext ctx) {
        return stmt switch {
            ExpressionStatementSyntax exprStmtSyntax => BuildFromExprStmt(exprStmtSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported statement: {stmt.Kind()}")
        };
    }

    public static Statement BuildFromExprStmt(ExpressionStatementSyntax exprStmt, TranspilationContext ctx) {
        var expr = exprStmt.Expression;

        switch (expr) {
            case AssignmentExpressionSyntax assignExpr: {
                var left = VarBuilder.BuildFromExpressionSyntax(assignExpr.Left, ctx);
                var right = ExpressionBuilder.BuildFromSyntax(assignExpr.Right, ctx);

                return new Assignment {
                    Vars = [left],
                    Expressions = [right]
                };
            }
        }

        throw new Exception($"Unhandled expression {expr.Kind()}");
    }
}