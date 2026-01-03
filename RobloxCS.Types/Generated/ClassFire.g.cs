#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Fire", RobloxNativeType.Instance)]
public partial class Fire : Instance  {
    public Color3 Color { get; } = default!;
    public bool Enabled { get; } = default!;
    public float Heat { get; } = default!;
    public Color3 SecondaryColor { get; } = default!;
    public float Size { get; } = default!;
    public float TimeScale { get; } = default!;
}
