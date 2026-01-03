#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SurfaceGui", RobloxNativeType.Instance)]
public partial class SurfaceGui : SurfaceGuiBase  {
    public bool AlwaysOnTop { get; } = default!;
    public float Brightness { get; } = default!;
    public Vector2 CanvasSize { get; } = default!;
    public bool ClipsDescendants { get; } = default!;
    public float LightInfluence { get; } = default!;
    public float MaxDistance { get; } = default!;
    public float PixelsPerStud { get; } = default!;
    public Enums.SurfaceGuiSizingMode SizingMode { get; } = default!;
    public float ToolPunchThroughDistance { get; } = default!;
    public float ZOffset { get; } = default!;
}
