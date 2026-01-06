#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SunRaysEffect", RobloxNativeType.Instance)]
public partial class SunRaysEffect : PostEffect  {
    public float Intensity { get; set; } = default!;
    public float Spread { get; set; } = default!;
}
