#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Sound", RobloxNativeType.Instance)]
public partial class Sound : Instance  {
    public string AudioContent { get; set; } = default!;
    public bool IsLoaded { get; set; } = default!;
    public NumberRange LoopRegion { get; set; } = default!;
    public bool Looped { get; set; } = default!;
    public bool PlayOnRemove { get; set; } = default!;
    public double PlaybackLoudness { get; set; } = default!;
    public NumberRange PlaybackRegion { get; set; } = default!;
    public bool PlaybackRegionsEnabled { get; set; } = default!;
    public float PlaybackSpeed { get; set; } = default!;
    public bool Playing { get; set; } = default!;
    public float RollOffMaxDistance { get; set; } = default!;
    public float RollOffMinDistance { get; set; } = default!;
    public Enums.RollOffMode RollOffMode { get; set; } = default!;
    public SoundGroup SoundGroup { get; set; } = default!;
    public string SoundId { get; set; } = default!;
    public double TimeLength { get; set; } = default!;
    public double TimePosition { get; set; } = default!;
    public float Volume { get; set; } = default!;
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
