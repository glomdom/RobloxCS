#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioChannelSplitter", RobloxNativeType.Instance)]
public partial class AudioChannelSplitter : Instance  {
    public Enums.AudioChannelLayout Layout { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
