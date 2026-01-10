#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioDeviceOutput", RobloxNativeType.Instance)]
public partial class AudioDeviceOutput : Instance  {
    public Player Player { get; set; } = default!;
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
