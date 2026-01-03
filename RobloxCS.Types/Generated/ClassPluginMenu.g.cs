#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginMenu", RobloxNativeType.Instance)]
public partial class PluginMenu : Instance  {
    public string Icon { get; } = default!;
    public string Title { get; } = default!;
}
