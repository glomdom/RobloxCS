#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioCompressor", RobloxNativeType.Instance)]
public partial class AudioCompressor : Instance  {
    public float Attack { get; set; } = default!;
    public bool Bypass { get; set; } = default!;
    public float MakeupGain { get; set; } = default!;
    public float Ratio { get; set; } = default!;
    public float Release { get; set; } = default!;
    public float Threshold { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
