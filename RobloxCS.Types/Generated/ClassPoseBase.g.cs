#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PoseBase", RobloxNativeType.Instance)]
public partial class PoseBase : Instance  {
    public Enums.PoseEasingDirection EasingDirection { get; set; } = default!;
    public Enums.PoseEasingStyle EasingStyle { get; set; } = default!;
    public float Weight { get; set; } = default!;
}
