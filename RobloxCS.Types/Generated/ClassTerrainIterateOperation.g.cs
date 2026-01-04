#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TerrainIterateOperation", RobloxNativeType.Instance)]
public partial class TerrainIterateOperation : Object  {
    public RBXScriptSignal CommitBlock() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Dictionary<string, object>> Ready { get; private set; } = null!;
}
