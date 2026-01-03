#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MaterialVariant", RobloxNativeType.Instance)]
public partial class MaterialVariant : Instance  {
    public Enums.AlphaMode AlphaMode { get; } = default!;
    public Enums.Material BaseMaterial { get; } = default!;
    public PhysicalProperties CustomPhysicalProperties { get; } = default!;
    public float EmissiveStrength { get; } = default!;
    public Color3 EmissiveTint { get; } = default!;
    public Enums.MaterialPattern MaterialPattern { get; } = default!;
    public float StudsPerTile { get; } = default!;
}
