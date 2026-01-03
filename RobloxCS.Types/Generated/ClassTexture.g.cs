#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Texture", RobloxNativeType.Instance)]
public partial class Texture : Decal  {
    public float OffsetStudsU { get; } = default!;
    public float OffsetStudsV { get; } = default!;
    public float StudsPerTileU { get; } = default!;
    public float StudsPerTileV { get; } = default!;
}
