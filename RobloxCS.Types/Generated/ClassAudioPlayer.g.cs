#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioPlayer", RobloxNativeType.Instance)]
public partial class AudioPlayer : Instance  {
    public string Asset { get; } = default!;
    public string AudioContent { get; } = default!;
    public bool AutoLoad { get; } = default!;
    public bool AutoPlay { get; } = default!;
    public bool IsPlaying { get; } = default!;
    public bool IsReady { get; } = default!;
    public NumberRange LoopRegion { get; } = default!;
    public bool Looping { get; } = default!;
    public NumberRange PlaybackRegion { get; } = default!;
    public double PlaybackSpeed { get; } = default!;
    public double TimeLength { get; } = default!;
    public double TimePosition { get; } = default!;
    public float Volume { get; } = default!;
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
