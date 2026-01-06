#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioReverb", RobloxNativeType.Instance)]
public partial class AudioReverb : Instance  {
    public bool Bypass { get; set; } = default!;
    public float DecayRatio { get; set; } = default!;
    public float DecayTime { get; set; } = default!;
    public float Density { get; set; } = default!;
    public float Diffusion { get; set; } = default!;
    public float DryLevel { get; set; } = default!;
    public float EarlyDelayTime { get; set; } = default!;
    public float HighCutFrequency { get; set; } = default!;
    public float LateDelayTime { get; set; } = default!;
    public float LowShelfFrequency { get; set; } = default!;
    public float LowShelfGain { get; set; } = default!;
    public float ReferenceFrequency { get; set; } = default!;
    public float WetLevel { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
