#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Smoke", RobloxNativeType.Instance)]
public partial class Smoke : Instance  {
    public Color3 Color { get; } = default!;
    public bool Enabled { get; } = default!;
    public float Opacity { get; } = default!;
    public float RiseVelocity { get; } = default!;
    public float Size { get; } = default!;
    public float TimeScale { get; } = default!;
}
