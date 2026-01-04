#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TerrainReadOperation", RobloxNativeType.Instance)]
public partial class TerrainReadOperation : Object  {
    public RBXScriptSignal<Dictionary<string, object>> Ready { get; private set; } = null!;
}
