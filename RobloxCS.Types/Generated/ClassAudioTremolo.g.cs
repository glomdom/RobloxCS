#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioTremolo", RobloxNativeType.Instance)]
public partial class AudioTremolo : Instance  {
    public bool Bypass { get; } = default!;
    public float Depth { get; } = default!;
    public float Duty { get; } = default!;
    public float Frequency { get; } = default!;
    public float Shape { get; } = default!;
    public float Skew { get; } = default!;
    public float Square { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
