#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PluginGui", RobloxNativeType.Instance)]
public partial class PluginGui : LayerCollector  {
    public string Title { get; set; } = default!;
    public void BindToClose() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
