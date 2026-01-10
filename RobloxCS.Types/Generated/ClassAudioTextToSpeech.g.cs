#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioTextToSpeech", RobloxNativeType.Instance)]
public partial class AudioTextToSpeech : Instance  {
    public bool IsLoaded { get; set; } = default!;
    public bool IsPlaying { get; } = default!;
    public bool Looping { get; set; } = default!;
    public float Pitch { get; set; } = default!;
    public float PlaybackSpeed { get; set; } = default!;
    public float Speed { get; set; } = default!;
    public string Text { get; set; } = default!;
    public double TimeLength { get; set; } = default!;
    public double TimePosition { get; set; } = default!;
    public string VoiceId { get; set; } = default!;
    public float Volume { get; set; } = default!;
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public void Pause() => ThrowHelper.ThrowTranspiledMethod();
    public void Play() => ThrowHelper.ThrowTranspiledMethod();
    public void Unload() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetWaveformAsync() => ThrowHelper.ThrowTranspiledMethod();
    public Enums.AssetFetchStatus LoadAsync() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal Ended { get; private set; } = null!;
    public RBXScriptSignal Looped { get; private set; } = null!;
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
