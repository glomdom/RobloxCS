#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioFilter", RobloxNativeType.Instance)]
public partial class AudioFilter : Instance  {
    public bool Bypass { get; } = default!;
    public Enums.AudioFilterType FilterType { get; } = default!;
    public float Frequency { get; } = default!;
    public float Gain { get; } = default!;
    public float Q { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetGainAt() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
