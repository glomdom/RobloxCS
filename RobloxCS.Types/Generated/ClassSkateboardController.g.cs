#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SkateboardController", RobloxNativeType.Instance)]
public partial class SkateboardController : Controller  {
    public float Steer { get; set; } = default!;
    public float Throttle { get; set; } = default!;
    public RBXScriptSignal<string> AxisChanged { get; private set; } = null!;
}
