#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioChorus", RobloxNativeType.Instance)]
public partial class AudioChorus : Instance  {
    public bool Bypass { get; set; } = default!;
    public float Depth { get; set; } = default!;
    public float Mix { get; set; } = default!;
    public float Rate { get; set; } = default!;
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
