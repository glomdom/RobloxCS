#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ConeHandleAdornment", RobloxNativeType.Instance)]
public partial class ConeHandleAdornment : HandleAdornment  {
    public float Height { get; set; } = default!;
    public bool Hollow { get; set; } = default!;
    public float Radius { get; set; } = default!;
    public Enums.AdornShading Shading { get; set; } = default!;
}
