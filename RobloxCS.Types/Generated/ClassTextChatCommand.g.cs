#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatCommand", RobloxNativeType.Instance)]
public partial class TextChatCommand : Instance  {
    public bool AutocompleteVisible { get; } = default!;
    public bool Enabled { get; } = default!;
    public string PrimaryAlias { get; } = default!;
    public string SecondaryAlias { get; } = default!;
}
