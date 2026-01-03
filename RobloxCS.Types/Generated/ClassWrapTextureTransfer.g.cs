#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WrapTextureTransfer", RobloxNativeType.Instance)]
public partial class WrapTextureTransfer : Instance  {
    public string ReferenceCageMeshContent { get; } = default!;
    public Vector2 UVMaxBound { get; } = default!;
    public Vector2 UVMinBound { get; } = default!;
}
