#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatMessageProperties", RobloxNativeType.Instance)]
public partial class TextChatMessageProperties : Instance  {
    public string PrefixText { get; set; } = default!;
    public string Text { get; set; } = default!;
    public string Translation { get; set; } = default!;
}
