#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BubbleChatConfiguration", RobloxNativeType.Instance)]
public partial class BubbleChatConfiguration : TextChatConfigurations  {
    public string AdorneeName { get; } = default!;
    public Color3 BackgroundColor3 { get; } = default!;
    public double BackgroundTransparency { get; } = default!;
    public float BubbleDuration { get; } = default!;
    public float BubblesSpacing { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.Font FontFace { get; } = default!;
    public Vector3 LocalPlayerStudsOffset { get; } = default!;
    public float MaxBubbles { get; } = default!;
    public float MaxDistance { get; } = default!;
    public float MinimizeDistance { get; } = default!;
    public bool TailVisible { get; } = default!;
    public Color3 TextColor3 { get; } = default!;
    public int TextSize { get; } = default!;
    public float VerticalStudsOffset { get; } = default!;
}
