#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BubbleChatMessageProperties", RobloxNativeType.Instance)]
public partial class BubbleChatMessageProperties : TextChatMessageProperties  {
    public Color3 BackgroundColor3 { get; } = default!;
    public double BackgroundTransparency { get; } = default!;
    public Enums.Font FontFace { get; } = default!;
    public bool TailVisible { get; } = default!;
    public Color3 TextColor3 { get; } = default!;
    public int TextSize { get; } = default!;
}
