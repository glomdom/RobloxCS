#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChannelTabsConfiguration", RobloxNativeType.Instance)]
public partial class ChannelTabsConfiguration : TextChatConfigurations  {
    public Vector2 AbsolutePosition { get; } = default!;
    public Vector2 AbsoluteSize { get; } = default!;
    public Color3 BackgroundColor3 { get; } = default!;
    public double BackgroundTransparency { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.Font FontFace { get; } = default!;
    public Color3 HoverBackgroundColor3 { get; } = default!;
    public Color3 SelectedTabTextColor3 { get; } = default!;
    public Color3 TextColor3 { get; } = default!;
    public int TextSize { get; } = default!;
    public Color3 TextStrokeColor3 { get; } = default!;
    public double TextStrokeTransparency { get; } = default!;
}
