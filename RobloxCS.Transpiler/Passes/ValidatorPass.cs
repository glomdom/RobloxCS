using RobloxCS.Transpiler.Walkers;
using Serilog;

namespace RobloxCS.Transpiler.Passes;

public sealed class ValidatorPass : IPass {
    public string Name => "Validator";
    public List<string> Diagnostics { get; } = [];

    public void Run(TranspilationContext ctx) {
        var walker = new ValidatorWalker(ctx);
        walker.Visit(ctx.Root);

        if (ctx.Options.ScriptType == ScriptType.Module) return;

        switch (walker) {
            case { FoundEntryPoint: true, IsAmbiguousEntryPoint: true }: Diagnostics.Add("Found an entry point but it is ambiguous."); break;
            case { FoundEntryPoint: true, IsAmbiguousEntryPoint: false }: {
                ctx.EntryPointName = walker.EntryPointNames[0];
                ctx.EntryPointClassName = walker.EntryPointClassName;

                Log.Verbose("Found entry point {EntryPointName}", ctx.EntryPointName);

                break;
            }

            default: {
                Diagnostics.Add("Missing entry point in non-module script"); // TODO: move this to Diagnostics struct

                break;
            }
        }
    }
}