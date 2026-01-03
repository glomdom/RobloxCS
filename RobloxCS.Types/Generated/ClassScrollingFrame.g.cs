#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ScrollingFrame", RobloxNativeType.Instance)]
public partial class ScrollingFrame : GuiObject  {
    public Vector2 AbsoluteCanvasSize { get; } = default!;
    public Vector2 AbsoluteWindowSize { get; } = default!;
    public Enums.AutomaticSize AutomaticCanvasSize { get; } = default!;
    public string BottomImage { get; } = default!;
    public string BottomImageContent { get; } = default!;
    public Vector2 CanvasPosition { get; } = default!;
    public UDim2 CanvasSize { get; } = default!;
    public Enums.ElasticBehavior ElasticBehavior { get; } = default!;
    public Enums.ScrollBarInset HorizontalScrollBarInset { get; } = default!;
    public string MidImage { get; } = default!;
    public string MidImageContent { get; } = default!;
    public Color3 ScrollBarImageColor3 { get; } = default!;
    public float ScrollBarImageTransparency { get; } = default!;
    public int ScrollBarThickness { get; } = default!;
    public Enums.ScrollingDirection ScrollingDirection { get; } = default!;
    public bool ScrollingEnabled { get; } = default!;
    public string TopImage { get; } = default!;
    public string TopImageContent { get; } = default!;
    public Enums.ScrollBarInset VerticalScrollBarInset { get; } = default!;
    public Enums.VerticalScrollBarPosition VerticalScrollBarPosition { get; } = default!;
}
