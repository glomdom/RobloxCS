#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BloomEffect", RobloxNativeType.Instance)]
public partial class BloomEffect : PostEffect  {
    public float Intensity { get; set; } = default!;
    public float Size { get; set; } = default!;
    public float Threshold { get; set; } = default!;
}
