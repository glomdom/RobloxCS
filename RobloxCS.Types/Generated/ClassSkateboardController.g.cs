#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SkateboardController", RobloxNativeType.Instance)]
public partial class SkateboardController : Controller  {
    public float Steer { get; } = default!;
    public float Throttle { get; } = default!;
}
