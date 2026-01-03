#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIAspectRatioConstraint", RobloxNativeType.Instance)]
public partial class UIAspectRatioConstraint : UIConstraint  {
    public float AspectRatio { get; } = default!;
    public Enums.AspectType AspectType { get; } = default!;
    public Enums.DominantAxis DominantAxis { get; } = default!;
}
