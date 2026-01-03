#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioPitchShifter", RobloxNativeType.Instance)]
public partial class AudioPitchShifter : Instance  {
    public bool Bypass { get; } = default!;
    public float Pitch { get; } = default!;
    public Enums.AudioWindowSize WindowSize { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
