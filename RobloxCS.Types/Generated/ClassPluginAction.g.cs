#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginAction", RobloxNativeType.Instance)]
public partial class PluginAction : Instance  {
    public string ActionId { get; set; } = default!;
    public bool AllowBinding { get; set; } = default!;
    public string StatusTip { get; set; } = default!;
    public string Text { get; } = default!;
}
