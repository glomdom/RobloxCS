#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Clouds", RobloxNativeType.Instance)]
public partial class Clouds : Instance  {
    public Color3 Color { get; set; } = default!;
    public float Cover { get; set; } = default!;
    public float Density { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
}
