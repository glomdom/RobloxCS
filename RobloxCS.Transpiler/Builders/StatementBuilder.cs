using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler.Builders;

public class StatementBuilder {
    public static Statement Transpile(StatementSyntax stmt, TranspilationContext ctx) {
        return stmt switch {
            ExpressionStatementSyntax exprStmtSyntax => BuildFromExprStmt(exprStmtSyntax, ctx),
            LocalDeclarationStatementSyntax localDeclStmtSyntax => BuildFromLocalDeclStmt(localDeclStmtSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported statement: {stmt.Kind()}"),
        };
    }

    private static Statement BuildFromLocalDeclStmt(LocalDeclarationStatementSyntax localDeclStmtSyntax, TranspilationContext ctx) {
        var decl = localDeclStmtSyntax.Declaration;
        var vars = decl.Variables;

        var varNames = vars.Select(vds => vds.Identifier.ValueText).ToList();
        var initExprSyntaxes = vars.Where(v => v.Initializer is not null).Select(v => v.Initializer!.Value);
        var initExprs = initExprSyntaxes.Select(s => ExpressionBuilder.BuildFromSyntax(s, ctx)).ToList();

        var typeSym = ctx.Semantics.CheckedGetTypeInfo(decl.Type);

        return LocalAssignment.OfSingleType(varNames, initExprs, SyntaxUtilities.BasicFromSymbol(typeSym));
    }

    private static Statement BuildFromExprStmt(ExpressionStatementSyntax exprStmt, TranspilationContext ctx) {
        var expr = exprStmt.Expression;

        switch (expr) {
            case AssignmentExpressionSyntax assignExpr: {
                var left = VarBuilder.BuildFromExpressionSyntax(assignExpr.Left, ctx);
                var right = ExpressionBuilder.BuildFromSyntax(assignExpr.Right, ctx);

                return new Assignment {
                    Vars = [left],
                    Expressions = [right],
                };
            }
        }

        throw new Exception($"Unhandled expression {expr.Kind()}");
    }
}