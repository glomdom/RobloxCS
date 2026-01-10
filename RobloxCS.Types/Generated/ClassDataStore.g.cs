#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStore", RobloxNativeType.Instance)]
public partial class DataStore : GlobalDataStore  {
    public LuaTuple GetVersionAsync() => ThrowHelper.ThrowTranspiledMethod();
    public LuaTuple GetVersionAtTimeAsync() => ThrowHelper.ThrowTranspiledMethod();
    public DataStoreKeyPages ListKeysAsync() => ThrowHelper.ThrowTranspiledMethod();
    public DataStoreVersionPages ListVersionsAsync() => ThrowHelper.ThrowTranspiledMethod();
    public void RemoveVersionAsync() => ThrowHelper.ThrowTranspiledMethod();
}
