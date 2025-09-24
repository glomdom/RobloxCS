using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;

namespace RobloxCS.Transpiler.Builders;

public class BlockBuilder {
    public static Block Build(BlockSyntax syntax, TranspilationContext ctx) {
        var block = Block.Empty();

        ctx.PushScope();
        var statements = syntax.Statements.Select(statement => StatementBuilder.Build(statement, ctx)).ToList();
        ctx.PopScope();

        block.Statements = statements;

        return block;
    }

    public static Block BuildFromStatement(StatementSyntax syntax, TranspilationContext ctx) {
        return syntax switch {
            BlockSyntax blockSyntax => BuildFromBlockStmt(blockSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported statement in Block.BuildFromStatement: {syntax.Kind()}"),
        };
    }

    private static Block BuildFromBlockStmt(BlockSyntax syntax, TranspilationContext ctx) {
        var stmts = syntax.Statements.Select(stmt => StatementBuilder.Build(stmt, ctx)).ToList();

        return new Block { Statements = stmts };
    }
}