using RobloxCS.Transpiler.Walkers;

namespace RobloxCS.Transpiler.Passes;

/// <summary>
/// Simple pass which maps C# -> Luua without using any logic.
/// The AST generated is either valid or invalid, depending
/// on how complicated the C# code is.
/// </summary>
public sealed class ConverterPass : IPass {
    public string Name => "Converter";

    public void Run(TranspilationContext ctx) {
        var walker = new ConverterWalker(ctx);

        walker.Visit(ctx.Root);
    }
}