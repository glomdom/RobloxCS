using System.Diagnostics;
using Serilog;

namespace RobloxCS.Transpiler.Passes;

public sealed class PassManager {
    public List<IPass> Passes { get; } = [];

    public void Register(IPass pass) => Passes.Add(pass);

    public void Run(TranspilationContext ctx) {
        var passesWatch = Stopwatch.StartNew();

        Log.Information("Starting passes");

        var passWatch = new Stopwatch();
        foreach (var pass in Passes) {
            passWatch.Restart();
            pass.Run(ctx);
            pass.PostRun(ctx);
            passWatch.Stop();

            Log.Debug("{Pass} finished in {ElapsedMs}ms", pass.GetType().Name, passWatch.ElapsedMilliseconds);
        }

        passesWatch.Stop();
        Log.Information("Finished passes in {ElapsedMs}ms", passesWatch.ElapsedMilliseconds);
    }
}