#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioDistortion", RobloxNativeType.Instance)]
public partial class AudioDistortion : Instance  {
    public bool Bypass { get; set; } = default!;
    public float Level { get; set; } = default!;
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
