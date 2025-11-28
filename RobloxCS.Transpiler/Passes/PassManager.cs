using Serilog;

namespace RobloxCS.Transpiler.Passes;

public sealed class PassManager {
    public List<IPass> Passes { get; } = [];

    public void Register(IPass pass) => Passes.Add(pass);

    public void Run(TranspilationContext ctx) {
        Log.Information("Starting passes");

        foreach (var pass in Passes) {
            Log.Debug("Running {Pass}", pass.GetType().Name);
            
            pass.Run(ctx);
        }
        
        Log.Information("Finished passes");
    }
}