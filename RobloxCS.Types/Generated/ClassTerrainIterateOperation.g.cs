#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TerrainIterateOperation", RobloxNativeType.Instance)]
public partial class TerrainIterateOperation : Object  {
    public RBXScriptSignal CommitBlock() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<Dictionary<string, object>> Ready { get; private set; } = null!;
}
