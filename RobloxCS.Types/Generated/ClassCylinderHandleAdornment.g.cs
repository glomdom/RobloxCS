#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CylinderHandleAdornment", RobloxNativeType.Instance)]
public partial class CylinderHandleAdornment : HandleAdornment  {
    public float Angle { get; } = default!;
    public float Height { get; } = default!;
    public float InnerRadius { get; } = default!;
    public float Radius { get; } = default!;
    public Enums.AdornShading Shading { get; } = default!;
}
