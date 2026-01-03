#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStoreObjectVersionInfo", RobloxNativeType.Instance)]
public partial class DataStoreObjectVersionInfo : Instance  {
    public int CreatedTime { get; } = default!;
    public bool IsDeleted { get; } = default!;
    public string Version { get; } = default!;
}
