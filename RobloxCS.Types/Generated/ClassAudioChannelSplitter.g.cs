#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioChannelSplitter", RobloxNativeType.Instance)]
public partial class AudioChannelSplitter : Instance  {
    public Enums.AudioChannelLayout Layout { get; set; } = default!;
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
