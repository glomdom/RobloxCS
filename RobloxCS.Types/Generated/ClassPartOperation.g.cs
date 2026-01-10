#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PartOperation", RobloxNativeType.Instance)]
public partial class PartOperation : TriangleMeshPart  {
    public Enums.RenderFidelity RenderFidelity { get; } = default!;
    public float SmoothingAngle { get; } = default!;
    public int TriangleCount { get; set; } = default!;
    public bool UsePartColor { get; set; } = default!;
    public void SubstituteGeometry() => ThrowHelper.ThrowTranspiledMethod();
}
