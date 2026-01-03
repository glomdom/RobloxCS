#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ConeHandleAdornment", RobloxNativeType.Instance)]
public partial class ConeHandleAdornment : HandleAdornment  {
    public float Height { get; } = default!;
    public bool Hollow { get; } = default!;
    public float Radius { get; } = default!;
    public Enums.AdornShading Shading { get; } = default!;
}
