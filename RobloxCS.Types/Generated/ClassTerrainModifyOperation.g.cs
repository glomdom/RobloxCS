#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TerrainModifyOperation", RobloxNativeType.Instance)]
public partial class TerrainModifyOperation : Object  {
    public RBXScriptSignal CommitBlock() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<Dictionary<string, object>> Ready { get; private set; } = null!;
}
