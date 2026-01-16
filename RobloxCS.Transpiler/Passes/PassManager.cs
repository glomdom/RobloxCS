using System.Diagnostics;
using RobloxCS.Common;
using Serilog;

namespace RobloxCS.Transpiler.Passes;

public sealed class PassManager {
    public List<IPass> Passes { get; } = [];

    public void Register(IPass pass) => Passes.Add(pass);

    public bool Run(TranspilationContext ctx) {
        var passesWatch = Stopwatch.StartNew();

        Log.Information("Starting passes");

        var passWatch = new Stopwatch();
        foreach (var pass in Passes) {
            passWatch.Restart();

            using (LoggerSetup.PushPass(pass.Name)) {
                pass.Run(ctx);
                pass.PostRun(ctx);
            }

            if (pass.Diagnostics.Count > 0) {
                pass.Diagnostics.ForEach(Log.Error);

                return false;
            }

            passWatch.Stop();

            Log.Debug("Pass {PassName} finished in {ElapsedMs}ms", pass.Name, passWatch.ElapsedMilliseconds);
        }

        passesWatch.Stop();
        Log.Information("Finished passes in {ElapsedMs}ms", passesWatch.ElapsedMilliseconds);

        return true;
    }
}