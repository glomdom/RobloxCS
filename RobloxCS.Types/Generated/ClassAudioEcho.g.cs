#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioEcho", RobloxNativeType.Instance)]
public partial class AudioEcho : Instance  {
    public bool Bypass { get; set; } = default!;
    public float DelayTime { get; set; } = default!;
    public float DryLevel { get; set; } = default!;
    public float Feedback { get; set; } = default!;
    public float RampTime { get; set; } = default!;
    public float WetLevel { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
