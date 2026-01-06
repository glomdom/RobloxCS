#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Texture", RobloxNativeType.Instance)]
public partial class Texture : Decal  {
    public float OffsetStudsU { get; set; } = default!;
    public float OffsetStudsV { get; set; } = default!;
    public float StudsPerTileU { get; set; } = default!;
    public float StudsPerTileV { get; set; } = default!;
}
