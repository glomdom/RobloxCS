#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChatInputBarConfiguration", RobloxNativeType.Instance)]
public partial class ChatInputBarConfiguration : TextChatConfigurations  {
    public Vector2 AbsolutePosition { get; } = default!;
    public Vector2 AbsoluteSize { get; } = default!;
    public bool AutocompleteEnabled { get; } = default!;
    public Color3 BackgroundColor3 { get; } = default!;
    public double BackgroundTransparency { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.Font FontFace { get; } = default!;
    public bool IsFocused { get; } = default!;
    public Enums.KeyCode KeyboardKeyCode { get; } = default!;
    public Color3 PlaceholderColor3 { get; } = default!;
    public TextChannel TargetTextChannel { get; } = default!;
    public TextBox TextBox { get; } = default!;
    public Color3 TextColor3 { get; } = default!;
    public int TextSize { get; } = default!;
    public Color3 TextStrokeColor3 { get; } = default!;
    public double TextStrokeTransparency { get; } = default!;
}
