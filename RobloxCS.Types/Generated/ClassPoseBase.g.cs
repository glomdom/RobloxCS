#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PoseBase", RobloxNativeType.Instance)]
public partial class PoseBase : Instance  {
    public Enums.PoseEasingDirection EasingDirection { get; } = default!;
    public Enums.PoseEasingStyle EasingStyle { get; } = default!;
    public float Weight { get; } = default!;
}
