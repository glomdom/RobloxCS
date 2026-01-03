#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BloomEffect", RobloxNativeType.Instance)]
public partial class BloomEffect : PostEffect  {
    public float Intensity { get; } = default!;
    public float Size { get; } = default!;
    public float Threshold { get; } = default!;
}
