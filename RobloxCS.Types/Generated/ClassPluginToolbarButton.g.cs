#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginToolbarButton", RobloxNativeType.Instance)]
public partial class PluginToolbarButton : Instance  {
    public bool ClickableWhenViewportHidden { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public string Icon { get; set; } = default!;
}
