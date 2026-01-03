#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioAnalyzer", RobloxNativeType.Instance)]
public partial class AudioAnalyzer : Instance  {
    public float PeakLevel { get; } = default!;
    public float RmsLevel { get; } = default!;
    public bool SpectrumEnabled { get; } = default!;
    public Enums.AudioWindowSize WindowSize { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetSpectrum() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
