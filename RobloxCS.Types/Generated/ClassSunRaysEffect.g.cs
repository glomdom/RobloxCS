#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SunRaysEffect", RobloxNativeType.Instance)]
public partial class SunRaysEffect : PostEffect  {
    public float Intensity { get; } = default!;
    public float Spread { get; } = default!;
}
