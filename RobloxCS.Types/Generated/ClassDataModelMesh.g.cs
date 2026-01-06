#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DataModelMesh", RobloxNativeType.Instance)]
public partial class DataModelMesh : Instance  {
    public Vector3 Offset { get; set; } = default!;
    public Vector3 Scale { get; set; } = default!;
    public Vector3 VertexColor { get; set; } = default!;
}
