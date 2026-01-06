#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RunService", RobloxNativeType.Service)]
public static partial class RunService {
    public static int FrameNumber { get; set; } = default!;
    public static void BindToRenderStep() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptConnection BindToSimulation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Enums.PredictionStatus GetPredictionStatus() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsClient() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsRunMode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsRunning() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsServer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsStudio() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetPredictionMode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void UnbindFromRenderStep() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<double> Heartbeat { get; private set; } = null!;
    public static RBXScriptSignal<int, List<object>> Misprediction { get; private set; } = null!;
    public static RBXScriptSignal<double> PostSimulation { get; private set; } = null!;
    public static RBXScriptSignal<double> PreAnimation { get; private set; } = null!;
    public static RBXScriptSignal<double> PreRender { get; private set; } = null!;
    public static RBXScriptSignal<double> PreSimulation { get; private set; } = null!;
    public static RBXScriptSignal<double> RenderStepped { get; private set; } = null!;
    public static RBXScriptSignal<double, double> Stepped { get; private set; } = null!;
}
