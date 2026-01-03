#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatMessageProperties", RobloxNativeType.Instance)]
public partial class TextChatMessageProperties : Instance  {
    public string PrefixText { get; } = default!;
    public string Text { get; } = default!;
    public string Translation { get; } = default!;
}
