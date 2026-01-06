#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BubbleChatConfiguration", RobloxNativeType.Instance)]
public partial class BubbleChatConfiguration : TextChatConfigurations  {
    public string AdorneeName { get; set; } = default!;
    public Color3 BackgroundColor3 { get; set; } = default!;
    public double BackgroundTransparency { get; set; } = default!;
    public float BubbleDuration { get; set; } = default!;
    public float BubblesSpacing { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public Enums.Font FontFace { get; set; } = default!;
    public Vector3 LocalPlayerStudsOffset { get; set; } = default!;
    public float MaxBubbles { get; set; } = default!;
    public float MaxDistance { get; set; } = default!;
    public float MinimizeDistance { get; set; } = default!;
    public bool TailVisible { get; set; } = default!;
    public Color3 TextColor3 { get; set; } = default!;
    public int TextSize { get; set; } = default!;
    public float VerticalStudsOffset { get; set; } = default!;
}
