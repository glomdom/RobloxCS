#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MaterialVariant", RobloxNativeType.Instance)]
public partial class MaterialVariant : Instance  {
    public Enums.AlphaMode AlphaMode { get; set; } = default!;
    public Enums.Material BaseMaterial { get; } = default!;
    public PhysicalProperties CustomPhysicalProperties { get; set; } = default!;
    public float EmissiveStrength { get; set; } = default!;
    public Color3 EmissiveTint { get; set; } = default!;
    public Enums.MaterialPattern MaterialPattern { get; set; } = default!;
    public float StudsPerTile { get; set; } = default!;
}
