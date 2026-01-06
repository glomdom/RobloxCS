#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Keyframe", RobloxNativeType.Instance)]
public partial class Keyframe : Instance  {
    public float Time { get; set; } = default!;
    public void AddMarker() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void AddPose() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetMarkers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetPoses() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveMarker() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemovePose() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
