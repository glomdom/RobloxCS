#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Sound", RobloxNativeType.Instance)]
public partial class Sound : Instance  {
    public string AudioContent { get; } = default!;
    public bool IsLoaded { get; } = default!;
    public NumberRange LoopRegion { get; } = default!;
    public bool Looped { get; } = default!;
    public bool PlayOnRemove { get; } = default!;
    public double PlaybackLoudness { get; } = default!;
    public NumberRange PlaybackRegion { get; } = default!;
    public bool PlaybackRegionsEnabled { get; } = default!;
    public float PlaybackSpeed { get; } = default!;
    public bool Playing { get; } = default!;
    public float RollOffMaxDistance { get; } = default!;
    public float RollOffMinDistance { get; } = default!;
    public Enums.RollOffMode RollOffMode { get; } = default!;
    public SoundGroup SoundGroup { get; } = default!;
    public string SoundId { get; } = default!;
    public double TimeLength { get; } = default!;
    public double TimePosition { get; } = default!;
    public float Volume { get; } = default!;
    public void Pause() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Resume() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Stop() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<string, int> DidLoop { get; private set; } = null!;
    public RBXScriptSignal<string> Ended { get; private set; } = null!;
    public RBXScriptSignal<string> Loaded { get; private set; } = null!;
    public RBXScriptSignal<string> Paused { get; private set; } = null!;
    public RBXScriptSignal<string> Played { get; private set; } = null!;
    public RBXScriptSignal<string> Resumed { get; private set; } = null!;
    public RBXScriptSignal<string> Stopped { get; private set; } = null!;
}
