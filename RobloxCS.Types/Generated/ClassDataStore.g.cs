#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStore", RobloxNativeType.Instance)]
public partial class DataStore : GlobalDataStore  {
    public LuaTuple GetVersionAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple GetVersionAtTimeAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public DataStoreKeyPages ListKeysAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public DataStoreVersionPages ListVersionsAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveVersionAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
