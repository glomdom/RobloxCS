#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GlobalSettings", RobloxNativeType.Instance)]
public partial class GlobalSettings : GenericSettings  {
    public bool GetFFlag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public string GetFVariable() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
