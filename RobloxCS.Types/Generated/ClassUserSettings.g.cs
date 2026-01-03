#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UserSettings", RobloxNativeType.Instance)]
public partial class UserSettings : GenericSettings  {
    public bool IsUserFeatureEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Reset() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
