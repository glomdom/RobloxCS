#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioPlayer", RobloxNativeType.Instance)]
public partial class AudioPlayer : Instance  {
    public string Asset { get; set; } = default!;
    public string AudioContent { get; set; } = default!;
    public bool AutoLoad { get; set; } = default!;
    public bool AutoPlay { get; set; } = default!;
    public bool IsPlaying { get; } = default!;
    public bool IsReady { get; set; } = default!;
    public NumberRange LoopRegion { get; set; } = default!;
    public bool Looping { get; set; } = default!;
    public NumberRange PlaybackRegion { get; set; } = default!;
    public double PlaybackSpeed { get; set; } = default!;
    public double TimeLength { get; set; } = default!;
    public double TimePosition { get; set; } = default!;
    public float Volume { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Stop() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetWaveformAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal Ended { get; private set; } = null!;
    public RBXScriptSignal Looped { get; private set; } = null!;
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
