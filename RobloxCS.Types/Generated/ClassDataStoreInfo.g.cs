#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStoreInfo", RobloxNativeType.Instance)]
public partial class DataStoreInfo : Instance  {
    public int CreatedTime { get; } = default!;
    public string DataStoreName { get; } = default!;
    public int UpdatedTime { get; } = default!;
}
