#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioEcho", RobloxNativeType.Instance)]
public partial class AudioEcho : Instance  {
    public bool Bypass { get; } = default!;
    public float DelayTime { get; } = default!;
    public float DryLevel { get; } = default!;
    public float Feedback { get; } = default!;
    public float RampTime { get; } = default!;
    public float WetLevel { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
