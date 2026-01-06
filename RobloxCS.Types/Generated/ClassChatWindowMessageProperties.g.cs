#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChatWindowMessageProperties", RobloxNativeType.Instance)]
public partial class ChatWindowMessageProperties : TextChatMessageProperties  {
    public Enums.Font FontFace { get; set; } = default!;
    public ChatWindowMessageProperties PrefixTextProperties { get; set; } = default!;
    public Color3 TextColor3 { get; set; } = default!;
    public int TextSize { get; set; } = default!;
    public Color3 TextStrokeColor3 { get; set; } = default!;
    public double TextStrokeTransparency { get; set; } = default!;
}
