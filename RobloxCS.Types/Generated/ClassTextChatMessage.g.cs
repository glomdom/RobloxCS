#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatMessage", RobloxNativeType.Instance)]
public partial class TextChatMessage : Instance  {
    public BubbleChatMessageProperties BubbleChatMessageProperties { get; set; } = default!;
    public ChatWindowMessageProperties ChatWindowMessageProperties { get; set; } = default!;
    public string MessageId { get; set; } = default!;
    public string Metadata { get; set; } = default!;
    public string PrefixText { get; set; } = default!;
    public Enums.TextChatMessageStatus Status { get; set; } = default!;
    public string Text { get; set; } = default!;
    public TextChannel TextChannel { get; set; } = default!;
    public TextSource TextSource { get; set; } = default!;
    public DateTime Timestamp { get; set; } = default!;
    public string Translation { get; set; } = default!;
}
