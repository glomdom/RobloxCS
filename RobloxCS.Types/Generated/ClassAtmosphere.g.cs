#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Atmosphere", RobloxNativeType.Instance)]
public partial class Atmosphere : Instance  {
    public Color3 Color { get; } = default!;
    public Color3 Decay { get; } = default!;
    public float Density { get; } = default!;
    public float Glare { get; } = default!;
    public float Haze { get; } = default!;
    public float Offset { get; } = default!;
}
