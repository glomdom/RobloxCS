#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChannelTabsConfiguration", RobloxNativeType.Instance)]
public partial class ChannelTabsConfiguration : TextChatConfigurations  {
    public Vector2 AbsolutePosition { get; set; } = default!;
    public Vector2 AbsoluteSize { get; set; } = default!;
    public Color3 BackgroundColor3 { get; set; } = default!;
    public double BackgroundTransparency { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public Enums.Font FontFace { get; set; } = default!;
    public Color3 HoverBackgroundColor3 { get; set; } = default!;
    public Color3 SelectedTabTextColor3 { get; set; } = default!;
    public Color3 TextColor3 { get; set; } = default!;
    public int TextSize { get; set; } = default!;
    public Color3 TextStrokeColor3 { get; set; } = default!;
    public double TextStrokeTransparency { get; set; } = default!;
}
