#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TerrainDetail", RobloxNativeType.Instance)]
public partial class TerrainDetail : Instance  {
    public float EmissiveStrength { get; } = default!;
    public Color3 EmissiveTint { get; } = default!;
    public Enums.TerrainFace Face { get; } = default!;
    public Enums.MaterialPattern MaterialPattern { get; } = default!;
    public float StudsPerTile { get; } = default!;
}
