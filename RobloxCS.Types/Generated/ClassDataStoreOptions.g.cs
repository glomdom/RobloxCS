#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStoreOptions", RobloxNativeType.Instance)]
public partial class DataStoreOptions : Instance  {
    public bool AllScopes { get; set; } = default!;
    public void SetExperimentalFeatures() => ThrowHelper.ThrowTranspiledMethod();
}
