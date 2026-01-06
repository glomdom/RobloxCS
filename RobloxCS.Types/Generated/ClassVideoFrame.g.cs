#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoFrame", RobloxNativeType.Instance)]
public partial class VideoFrame : GuiObject  {
    public bool IsLoaded { get; set; } = default!;
    public bool Looped { get; set; } = default!;
    public bool Playing { get; set; } = default!;
    public Vector2 Resolution { get; set; } = default!;
    public double TimeLength { get; set; } = default!;
    public double TimePosition { get; set; } = default!;
    public string Video { get; set; } = default!;
    public string VideoContent { get; set; } = default!;
    public float Volume { get; set; } = default!;
    public void Pause() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<string> DidLoop { get; private set; } = null!;
    public RBXScriptSignal<string> Ended { get; private set; } = null!;
    public RBXScriptSignal<string> Loaded { get; private set; } = null!;
    public RBXScriptSignal<string> Paused { get; private set; } = null!;
    public RBXScriptSignal<string> Played { get; private set; } = null!;
}
