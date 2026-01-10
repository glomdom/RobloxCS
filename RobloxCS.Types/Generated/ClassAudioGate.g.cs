#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioGate", RobloxNativeType.Instance)]
public partial class AudioGate : Instance  {
    public float Attack { get; set; } = default!;
    public bool Bypass { get; set; } = default!;
    public float Release { get; set; } = default!;
    public NumberRange Threshold { get; set; } = default!;
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
