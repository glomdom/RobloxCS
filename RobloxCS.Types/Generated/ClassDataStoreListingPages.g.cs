#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStoreListingPages", RobloxNativeType.Instance)]
public partial class DataStoreListingPages : Pages  {
    public string Cursor { get; set; } = default!;
}
