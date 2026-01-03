#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStoreKeyInfo", RobloxNativeType.Instance)]
public partial class DataStoreKeyInfo : Instance  {
    public int CreatedTime { get; } = default!;
    public int UpdatedTime { get; } = default!;
    public string Version { get; } = default!;
    public Dictionary<string, object> GetMetadata() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetUserIds() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
