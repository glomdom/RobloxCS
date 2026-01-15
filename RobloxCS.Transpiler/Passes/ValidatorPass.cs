using RobloxCS.Transpiler.Walkers;
using Serilog;

namespace RobloxCS.Transpiler.Passes;

public sealed class ValidatorPass : IPass {
    public string Name => "Validator";

    public void Run(TranspilationContext ctx) {
        var walker = new ValidatorWalker(ctx);
        walker.Visit(ctx.Root);

        if (ctx.Options.ScriptType != ScriptType.Module) {
            if (walker is { FoundEntryPoint: true, IsAmbiguousEntryPoint: true }) {
                Log.Error("Found an entry point but it is ambiguous.");
            } else if (walker is { FoundEntryPoint: true, IsAmbiguousEntryPoint: false }) {
                ctx.EntryPointName = walker.EntryPointNames[0];

                Log.Verbose("Found entry point {EntryPointName}", ctx.EntryPointName);
            } else {
                Log.Error("Missing entry point in non-module script");
            }
        }
    }
}