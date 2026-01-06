#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ColorCorrectionEffect", RobloxNativeType.Instance)]
public partial class ColorCorrectionEffect : PostEffect  {
    public float Brightness { get; set; } = default!;
    public float Contrast { get; set; } = default!;
    public float Saturation { get; set; } = default!;
    public Color3 TintColor { get; set; } = default!;
}
