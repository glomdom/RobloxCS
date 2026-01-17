using RobloxCS.Transpiler.Helpers;
using Serilog;

namespace RobloxCS.Transpiler.Passes;

public sealed class ProloguePass : IPass {
    public string Name => "Prologue";
    public List<string> Diagnostics { get; } = [];

    public void Run(TranspilationContext ctx) {
        var entryName = ctx.EntryPointName!;
        var instantiateExpr = ExpressionHelpers.DirectFunctionCall(ctx.EntryPointClassName, "new");
        var instantiateBind = StatementHelpers.UntypedLocalAssignment("__entrypointCls", instantiateExpr);

        Log.Debug("Created statement for instantiating class containing entry point");
        ctx.RootBlock.AddStatement(instantiateBind);

        var callEntryPoint = StatementHelpers.SimpleMethodCall("__entrypointCls", entryName);

        Log.Debug("Created statement for calling entry point");
        ctx.RootBlock.AddStatement(callEntryPoint);
    }
}