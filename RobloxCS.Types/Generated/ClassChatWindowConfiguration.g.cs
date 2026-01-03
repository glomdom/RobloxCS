#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChatWindowConfiguration", RobloxNativeType.Instance)]
public partial class ChatWindowConfiguration : TextChatConfigurations  {
    public Vector2 AbsolutePosition { get; } = default!;
    public Vector2 AbsoluteSize { get; } = default!;
    public Color3 BackgroundColor3 { get; } = default!;
    public double BackgroundTransparency { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.Font FontFace { get; } = default!;
    public float HeightScale { get; } = default!;
    public Enums.HorizontalAlignment HorizontalAlignment { get; } = default!;
    public Color3 TextColor3 { get; } = default!;
    public int TextSize { get; } = default!;
    public Color3 TextStrokeColor3 { get; } = default!;
    public double TextStrokeTransparency { get; } = default!;
    public Enums.VerticalAlignment VerticalAlignment { get; } = default!;
    public float WidthScale { get; } = default!;
    public ChatWindowMessageProperties DeriveNewMessageProperties() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
