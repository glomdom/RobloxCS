#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Path", RobloxNativeType.Instance)]
public partial class Path : Instance  {
    public Enums.PathStatus Status { get; } = default!;
    public List<object> GetWaypoints() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int CheckOcclusionAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ComputeAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
