#nullable enable
namespace RobloxCS.Types;
[RobloxNative("FileMesh", RobloxNativeType.Instance)]
public partial class FileMesh : DataModelMesh  {
    public string MeshId { get; } = default!;
    public string TextureId { get; } = default!;
}
