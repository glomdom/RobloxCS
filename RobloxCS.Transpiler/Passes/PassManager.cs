using System.Diagnostics;
using RobloxCS.Common;
using RobloxCS.HIR;
using RobloxCS.Transpiler.Builders.HIR;
using Serilog;
using Spectre.Console;

namespace RobloxCS.Transpiler.Passes;

public sealed class PassManager {
    public IReadOnlyList<IPass> Passes => _passes;

    private readonly List<IPass> _passes = [];

    public void Register(IPass pass) => _passes.Add(pass);

    public HirModule? Start(TranspilationContext ctx) {
        Log.Information("Starting passes");

        var module = new HirBuilder(ctx).Build();
        if (ctx.Diagnostics.HasError) {
            foreach (var x in ctx.Diagnostics.RenderMarkup()) {
                AnsiConsole.MarkupLine(x);
            }

            return null;
        }

        foreach (var pass in _passes) {
            using (LoggerSetup.PushPass(pass.Name)) {
                module = pass.Run(module, ctx);
            }

            if (pass.Diagnostics.Count > 0) {
                pass.Diagnostics.ForEach(Log.Error);

                return null;
            }

            Log.Debug("Pass {PassName} finished", pass.Name);
        }
        
        foreach (var x in ctx.Diagnostics.RenderMarkup()) {
            AnsiConsole.MarkupLine(x);
        }

        Log.Information("Finished passes");

        return module;
    }
}