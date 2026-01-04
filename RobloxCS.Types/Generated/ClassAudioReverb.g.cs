#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioReverb", RobloxNativeType.Instance)]
public partial class AudioReverb : Instance  {
    public bool Bypass { get; } = default!;
    public float DecayRatio { get; } = default!;
    public float DecayTime { get; } = default!;
    public float Density { get; } = default!;
    public float Diffusion { get; } = default!;
    public float DryLevel { get; } = default!;
    public float EarlyDelayTime { get; } = default!;
    public float HighCutFrequency { get; } = default!;
    public float LateDelayTime { get; } = default!;
    public float LowShelfFrequency { get; } = default!;
    public float LowShelfGain { get; } = default!;
    public float ReferenceFrequency { get; } = default!;
    public float WetLevel { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
