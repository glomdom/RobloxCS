#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NumberPose", RobloxNativeType.Instance)]
public partial class NumberPose : PoseBase  {
    public double Value { get; } = default!;
}
