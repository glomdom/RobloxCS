#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SurfaceAppearance", RobloxNativeType.Instance)]
public partial class SurfaceAppearance : Instance  {
    public Enums.AlphaMode AlphaMode { get; set; } = default!;
    public Color3 Color { get; set; } = default!;
    public string EmissiveMaskContent { get; } = default!;
    public float EmissiveStrength { get; set; } = default!;
    public Color3 EmissiveTint { get; set; } = default!;
}
