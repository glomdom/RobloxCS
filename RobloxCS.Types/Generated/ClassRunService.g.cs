#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RunService", RobloxNativeType.Service)]
public static partial class RunService {
    public static int FrameNumber { get; set; } = default!;
    public static void BindToRenderStep() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptConnection BindToSimulation() => ThrowHelper.ThrowTranspiledMethod();
    public static Enums.PredictionStatus GetPredictionStatus() => ThrowHelper.ThrowTranspiledMethod();
    public static bool IsClient() => ThrowHelper.ThrowTranspiledMethod();
    public static bool IsRunMode() => ThrowHelper.ThrowTranspiledMethod();
    public static bool IsRunning() => ThrowHelper.ThrowTranspiledMethod();
    public static bool IsServer() => ThrowHelper.ThrowTranspiledMethod();
    public static bool IsStudio() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetPredictionMode() => ThrowHelper.ThrowTranspiledMethod();
    public static void UnbindFromRenderStep() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<double> Heartbeat { get; private set; } = null!;
    public static RBXScriptSignal<int, List<object>> Misprediction { get; private set; } = null!;
    public static RBXScriptSignal<double> PostSimulation { get; private set; } = null!;
    public static RBXScriptSignal<double> PreAnimation { get; private set; } = null!;
    public static RBXScriptSignal<double> PreRender { get; private set; } = null!;
    public static RBXScriptSignal<double> PreSimulation { get; private set; } = null!;
    public static RBXScriptSignal<double> RenderStepped { get; private set; } = null!;
    public static RBXScriptSignal<double, double> Stepped { get; private set; } = null!;
}
