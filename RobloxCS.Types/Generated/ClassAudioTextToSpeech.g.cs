#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioTextToSpeech", RobloxNativeType.Instance)]
public partial class AudioTextToSpeech : Instance  {
    public bool IsLoaded { get; } = default!;
    public bool IsPlaying { get; } = default!;
    public bool Looping { get; } = default!;
    public float Pitch { get; } = default!;
    public float PlaybackSpeed { get; } = default!;
    public float Speed { get; } = default!;
    public string Text { get; } = default!;
    public double TimeLength { get; } = default!;
    public double TimePosition { get; } = default!;
    public string VoiceId { get; } = default!;
    public float Volume { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Pause() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Unload() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetWaveformAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Enums.AssetFetchStatus LoadAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
