#nullable enable
namespace RobloxCS.Types;
[RobloxNative("OrderedDataStore", RobloxNativeType.Instance)]
public partial class OrderedDataStore : GlobalDataStore  {
    public DataStorePages GetSortedAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
