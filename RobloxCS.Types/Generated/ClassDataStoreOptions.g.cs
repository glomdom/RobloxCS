#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataStoreOptions", RobloxNativeType.Instance)]
public partial class DataStoreOptions : Instance  {
    public bool AllScopes { get; } = default!;
    public void SetExperimentalFeatures() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
