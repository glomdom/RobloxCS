#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Path", RobloxNativeType.Instance)]
public partial class Path : Instance  {
    public Enums.PathStatus Status { get; set; } = default!;
    public List<object> GetWaypoints() => ThrowHelper.ThrowTranspiledMethod();
    public int CheckOcclusionAsync() => ThrowHelper.ThrowTranspiledMethod();
    public void ComputeAsync() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<int> Blocked { get; private set; } = null!;
    public RBXScriptSignal<int> Unblocked { get; private set; } = null!;
}
