#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Pose", RobloxNativeType.Instance)]
public partial class Pose : PoseBase  {
    public CFrame CFrame { get; } = default!;
    public void AddSubPose() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetSubPoses() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveSubPose() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
