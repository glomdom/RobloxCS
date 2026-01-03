#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TriangleMeshPart", RobloxNativeType.Instance)]
public partial class TriangleMeshPart : BasePart  {
    public Enums.CollisionFidelity CollisionFidelity { get; } = default!;
    public Enums.FluidFidelity FluidFidelity { get; } = default!;
    public Vector3 MeshSize { get; } = default!;
}
