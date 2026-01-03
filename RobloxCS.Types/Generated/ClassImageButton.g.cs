#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ImageButton", RobloxNativeType.Instance)]
public partial class ImageButton : GuiButton  {
    public string HoverImage { get; } = default!;
    public string HoverImageContent { get; } = default!;
    public string Image { get; } = default!;
    public Color3 ImageColor3 { get; } = default!;
    public string ImageContent { get; } = default!;
    public Vector2 ImageRectOffset { get; } = default!;
    public Vector2 ImageRectSize { get; } = default!;
    public float ImageTransparency { get; } = default!;
    public bool IsLoaded { get; } = default!;
    public string PressedImage { get; } = default!;
    public string PressedImageContent { get; } = default!;
    public Enums.ResamplerMode ResampleMode { get; } = default!;
    public Enums.ScaleType ScaleType { get; } = default!;
    public Rect SliceCenter { get; } = default!;
    public float SliceScale { get; } = default!;
    public UDim2 TileSize { get; } = default!;
}
