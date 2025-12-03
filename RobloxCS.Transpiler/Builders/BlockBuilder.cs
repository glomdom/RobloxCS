using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.Transpiler.Helpers;

namespace RobloxCS.Transpiler.Builders;

public static class BlockBuilder {
    public static Block Build(BlockSyntax syntax, TranspilationContext ctx) {
        var block = BlockHelpers.Empty();
        var result = syntax.Statements.Select(s => StatementBuilder.Build(s, ctx));
        block.Statements = result.ToList();

        return block;
    }

    public static Block BuildFromStatement(StatementSyntax syntax, TranspilationContext ctx) {
        return syntax switch {
            BlockSyntax blockSyntax => BuildFromBlockStmt(blockSyntax, ctx),

            _ => throw new NotSupportedException($"Unsupported statement in Block.BuildFromStatement: {syntax.Kind()}"),
        };
    }

    private static Block BuildFromBlockStmt(BlockSyntax syntax, TranspilationContext ctx) {
        var result = syntax.Statements.Select(stmt => StatementBuilder.Build(stmt, ctx));

        return new Block { Statements = result.ToList() };
    }
}