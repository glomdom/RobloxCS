#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStoreKey", RobloxNativeType.Instance)]
public partial class DataStoreKey : Instance  {
    public string KeyName { get; set; } = default!;
}
