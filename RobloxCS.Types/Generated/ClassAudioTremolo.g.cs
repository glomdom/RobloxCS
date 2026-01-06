#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioTremolo", RobloxNativeType.Instance)]
public partial class AudioTremolo : Instance  {
    public bool Bypass { get; set; } = default!;
    public float Depth { get; set; } = default!;
    public float Duty { get; set; } = default!;
    public float Frequency { get; set; } = default!;
    public float Shape { get; set; } = default!;
    public float Skew { get; set; } = default!;
    public float Square { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
