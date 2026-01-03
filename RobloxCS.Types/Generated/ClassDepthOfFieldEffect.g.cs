#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DepthOfFieldEffect", RobloxNativeType.Instance)]
public partial class DepthOfFieldEffect : PostEffect  {
    public float FarIntensity { get; } = default!;
    public float FocusDistance { get; } = default!;
    public float InFocusRadius { get; } = default!;
    public float NearIntensity { get; } = default!;
}
