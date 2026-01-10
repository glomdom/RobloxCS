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
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
