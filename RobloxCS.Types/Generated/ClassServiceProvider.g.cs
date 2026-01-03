#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ServiceProvider", RobloxNativeType.Instance)]
public partial class ServiceProvider : Instance  {
    public Instance FindService() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance GetService() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
