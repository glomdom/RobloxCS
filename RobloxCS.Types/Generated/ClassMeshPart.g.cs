#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MeshPart", RobloxNativeType.Instance)]
public partial class MeshPart : TriangleMeshPart  {
    public bool DoubleSided { get; } = default!;
    public string MeshContent { get; } = default!;
    public string MeshId { get; } = default!;
    public Enums.RenderFidelity RenderFidelity { get; } = default!;
    public string TextureContent { get; set; } = default!;
    public string TextureID { get; set; } = default!;
    public void ApplyMesh() => ThrowHelper.ThrowTranspiledMethod();
}
