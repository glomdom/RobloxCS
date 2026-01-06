#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TerrainDetail", RobloxNativeType.Instance)]
public partial class TerrainDetail : Instance  {
    public float EmissiveStrength { get; set; } = default!;
    public Color3 EmissiveTint { get; set; } = default!;
    public Enums.TerrainFace Face { get; set; } = default!;
    public Enums.MaterialPattern MaterialPattern { get; set; } = default!;
    public float StudsPerTile { get; set; } = default!;
}
