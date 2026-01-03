#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginAction", RobloxNativeType.Instance)]
public partial class PluginAction : Instance  {
    public string ActionId { get; } = default!;
    public bool AllowBinding { get; } = default!;
    public string StatusTip { get; } = default!;
    public string Text { get; } = default!;
}
