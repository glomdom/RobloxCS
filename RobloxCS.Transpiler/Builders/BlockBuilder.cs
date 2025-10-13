using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using Serilog;

namespace RobloxCS.Transpiler.Builders;

public static class BlockBuilder {
    public static Block Build(BlockSyntax syntax, TranspilationContext ctx) {
        var block = Block.Empty();

        ctx.PushScope();
        var result = syntax.Statements.Select(statement => {
            Log.Debug("Visiting statement {Kind} in block", statement.Kind());

            return StatementBuilder.Build(statement, ctx);
        }).Flatten();

        ctx.PopScope();

        block.Statements = result.Statements;

        return block;
    }

    public static Block BuildFromStatement(StatementSyntax syntax, TranspilationContext ctx) {
        return syntax switch {
            BlockSyntax blockSyntax => BuildFromBlockStmt(blockSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported statement in Block.BuildFromStatement: {syntax.Kind()}"),
        };
    }

    private static Block BuildFromBlockStmt(BlockSyntax syntax, TranspilationContext ctx) {
        var result = syntax.Statements.Select(stmt => StatementBuilder.Build(stmt, ctx)).Flatten();

        return new Block { Statements = result.Statements };
    }
}