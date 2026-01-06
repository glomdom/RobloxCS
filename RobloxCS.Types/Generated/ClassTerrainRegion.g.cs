#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TerrainRegion", RobloxNativeType.Instance)]
public partial class TerrainRegion : Instance  {
    public Vector3 SizeInCells { get; set; } = default!;
}
