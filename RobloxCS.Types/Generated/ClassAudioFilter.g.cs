#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioFilter", RobloxNativeType.Instance)]
public partial class AudioFilter : Instance  {
    public bool Bypass { get; set; } = default!;
    public Enums.AudioFilterType FilterType { get; set; } = default!;
    public float Frequency { get; set; } = default!;
    public float Gain { get; set; } = default!;
    public float Q { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetGainAt() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
