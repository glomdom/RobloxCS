#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Clouds", RobloxNativeType.Instance)]
public partial class Clouds : Instance  {
    public Color3 Color { get; } = default!;
    public float Cover { get; } = default!;
    public float Density { get; } = default!;
    public bool Enabled { get; } = default!;
}
