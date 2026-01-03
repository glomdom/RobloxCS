#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChatWindowMessageProperties", RobloxNativeType.Instance)]
public partial class ChatWindowMessageProperties : TextChatMessageProperties  {
    public Enums.Font FontFace { get; } = default!;
    public ChatWindowMessageProperties PrefixTextProperties { get; } = default!;
    public Color3 TextColor3 { get; } = default!;
    public int TextSize { get; } = default!;
    public Color3 TextStrokeColor3 { get; } = default!;
    public double TextStrokeTransparency { get; } = default!;
}
