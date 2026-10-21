using RobloxCS.Common;
using RobloxCS.HIR;
using Serilog;

namespace RobloxCS.Passes;

public sealed class PassManager {
    public IReadOnlyList<IPass> Passes => _passes;

    private readonly List<IPass> _passes = [];

    public void Register(IPass pass) => _passes.Add(pass);

    /// <summary>
    /// Runs every registered pass in order. Stops early once a pass reports an error.
    /// </summary>
    public HirModule Run(HirModule module, PassContext ctx) {
        Log.Information("Starting passes");

        foreach (var pass in _passes) {
            using (LoggerSetup.PushPass(pass.Name)) {
                module = pass.Run(module, ctx);
            }

            Log.Debug("Pass {PassName} finished", pass.Name);

            if (ctx.Diagnostics.HasErrors) {
                Log.Debug("Pass {PassName} reported errors, stopping", pass.Name);

                break;
            }
        }

        Log.Information("Finished passes");

        return module;
    }
}
