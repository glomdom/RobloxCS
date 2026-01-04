#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioCompressor", RobloxNativeType.Instance)]
public partial class AudioCompressor : Instance  {
    public float Attack { get; } = default!;
    public bool Bypass { get; } = default!;
    public float MakeupGain { get; } = default!;
    public float Ratio { get; } = default!;
    public float Release { get; } = default!;
    public float Threshold { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
