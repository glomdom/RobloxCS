#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoFrame", RobloxNativeType.Instance)]
public partial class VideoFrame : GuiObject  {
    public bool IsLoaded { get; } = default!;
    public bool Looped { get; } = default!;
    public bool Playing { get; } = default!;
    public Vector2 Resolution { get; } = default!;
    public double TimeLength { get; } = default!;
    public double TimePosition { get; } = default!;
    public string Video { get; } = default!;
    public string VideoContent { get; } = default!;
    public float Volume { get; } = default!;
    public void Pause() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
