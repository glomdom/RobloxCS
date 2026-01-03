#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginToolbarButton", RobloxNativeType.Instance)]
public partial class PluginToolbarButton : Instance  {
    public bool ClickableWhenViewportHidden { get; } = default!;
    public bool Enabled { get; } = default!;
    public string Icon { get; } = default!;
}
