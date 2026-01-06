#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoPlayer", RobloxNativeType.Instance)]
public partial class VideoPlayer : Instance  {
    public bool AutoLoadInStudio { get; } = default!;
    public bool AutoPlayInStudio { get; } = default!;
    public bool IsLoaded { get; set; } = default!;
    public bool IsPlaying { get; set; } = default!;
    public bool Looping { get; set; } = default!;
    public float PlaybackSpeed { get; set; } = default!;
    public Vector2 Resolution { get; set; } = default!;
    public double TimeLength { get; set; } = default!;
    public double TimePosition { get; set; } = default!;
    public string VideoContent { get; set; } = default!;
    public float Volume { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Pause() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Unload() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Enums.AssetFetchStatus LoadAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal DidEnd { get; private set; } = null!;
    public RBXScriptSignal DidLoop { get; private set; } = null!;
    public RBXScriptSignal<Enums.AssetFetchStatus> PlayFailed { get; private set; } = null!;
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
