#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Light", RobloxNativeType.Instance)]
public partial class Light : Instance  {
    public float Brightness { get; set; } = default!;
    public Color3 Color { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public bool Shadows { get; set; } = default!;
}
