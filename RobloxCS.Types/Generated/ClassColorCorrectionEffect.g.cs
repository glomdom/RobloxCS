#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ColorCorrectionEffect", RobloxNativeType.Instance)]
public partial class ColorCorrectionEffect : PostEffect  {
    public float Brightness { get; } = default!;
    public float Contrast { get; } = default!;
    public float Saturation { get; } = default!;
    public Color3 TintColor { get; } = default!;
}
