#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Light", RobloxNativeType.Instance)]
public partial class Light : Instance  {
    public float Brightness { get; } = default!;
    public Color3 Color { get; } = default!;
    public bool Enabled { get; } = default!;
    public bool Shadows { get; } = default!;
}
