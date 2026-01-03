#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SphereHandleAdornment", RobloxNativeType.Instance)]
public partial class SphereHandleAdornment : HandleAdornment  {
    public float Radius { get; } = default!;
    public Enums.AdornShading Shading { get; } = default!;
}
