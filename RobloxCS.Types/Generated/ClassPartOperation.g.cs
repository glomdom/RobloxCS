#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PartOperation", RobloxNativeType.Instance)]
public partial class PartOperation : TriangleMeshPart  {
    public Enums.RenderFidelity RenderFidelity { get; } = default!;
    public float SmoothingAngle { get; } = default!;
    public int TriangleCount { get; } = default!;
    public bool UsePartColor { get; } = default!;
    public void SubstituteGeometry() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
