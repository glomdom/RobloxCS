#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SurfaceAppearance", RobloxNativeType.Instance)]
public partial class SurfaceAppearance : Instance  {
    public Enums.AlphaMode AlphaMode { get; } = default!;
    public Color3 Color { get; } = default!;
    public string EmissiveMaskContent { get; } = default!;
    public float EmissiveStrength { get; } = default!;
    public Color3 EmissiveTint { get; } = default!;
}
