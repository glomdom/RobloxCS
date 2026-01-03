#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BindableEvent", RobloxNativeType.Instance)]
public partial class BindableEvent : Instance  {
    public void Fire() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
