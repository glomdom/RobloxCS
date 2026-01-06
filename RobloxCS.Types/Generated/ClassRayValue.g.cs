#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RayValue", RobloxNativeType.Instance)]
public partial class RayValue : ValueBase  {
    public Ray Value { get; set; } = default!;
    new public RBXScriptSignal<Ray> Changed { get; private set; } = null!;
}
