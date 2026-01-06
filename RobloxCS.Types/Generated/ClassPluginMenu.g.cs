#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginMenu", RobloxNativeType.Instance)]
public partial class PluginMenu : Instance  {
    public string Icon { get; set; } = default!;
    public string Title { get; set; } = default!;
}
