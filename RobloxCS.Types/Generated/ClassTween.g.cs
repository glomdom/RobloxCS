#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Tween", RobloxNativeType.Instance)]
public partial class Tween : TweenBase  {
    public Instance Instance { get; set; } = default!;
    public TweenInfo TweenInfo { get; set; } = default!;
}
