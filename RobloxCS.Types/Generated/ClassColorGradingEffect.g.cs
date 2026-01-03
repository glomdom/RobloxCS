#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ColorGradingEffect", RobloxNativeType.Instance)]
public partial class ColorGradingEffect : PostEffect  {
    public Enums.TonemapperPreset TonemapperPreset { get; } = default!;
}
