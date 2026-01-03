#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatMessage", RobloxNativeType.Instance)]
public partial class TextChatMessage : Instance  {
    public BubbleChatMessageProperties BubbleChatMessageProperties { get; } = default!;
    public ChatWindowMessageProperties ChatWindowMessageProperties { get; } = default!;
    public string MessageId { get; } = default!;
    public string Metadata { get; } = default!;
    public string PrefixText { get; } = default!;
    public Enums.TextChatMessageStatus Status { get; } = default!;
    public string Text { get; } = default!;
    public TextChannel TextChannel { get; } = default!;
    public TextSource TextSource { get; } = default!;
    public DateTime Timestamp { get; } = default!;
    public string Translation { get; } = default!;
}
