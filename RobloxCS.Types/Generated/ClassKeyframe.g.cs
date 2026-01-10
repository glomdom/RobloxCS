#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Keyframe", RobloxNativeType.Instance)]
public partial class Keyframe : Instance  {
    public float Time { get; set; } = default!;
    public void AddMarker() => ThrowHelper.ThrowTranspiledMethod();
    public void AddPose() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetMarkers() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetPoses() => ThrowHelper.ThrowTranspiledMethod();
    public void RemoveMarker() => ThrowHelper.ThrowTranspiledMethod();
    public void RemovePose() => ThrowHelper.ThrowTranspiledMethod();
}
