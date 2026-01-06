#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatCommand", RobloxNativeType.Instance)]
public partial class TextChatCommand : Instance  {
    public bool AutocompleteVisible { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public string PrimaryAlias { get; set; } = default!;
    public string SecondaryAlias { get; set; } = default!;
    public RBXScriptSignal<TextSource, string> Triggered { get; private set; } = null!;
}
