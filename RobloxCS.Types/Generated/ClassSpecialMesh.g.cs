#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SpecialMesh", RobloxNativeType.Instance)]
public partial class SpecialMesh : FileMesh  {
    public Enums.MeshType MeshType { get; set; } = default!;
}
