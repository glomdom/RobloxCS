#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BoxHandleAdornment", RobloxNativeType.Instance)]
public partial class BoxHandleAdornment : HandleAdornment  {
    public Enums.AdornShading Shading { get; set; } = default!;
    public Vector3 Size { get; set; } = default!;
}
