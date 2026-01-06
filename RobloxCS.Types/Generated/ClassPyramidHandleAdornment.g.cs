#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PyramidHandleAdornment", RobloxNativeType.Instance)]
public partial class PyramidHandleAdornment : HandleAdornment  {
    public float Height { get; set; } = default!;
    public Enums.AdornShading Shading { get; set; } = default!;
    public int Sides { get; set; } = default!;
    public float Size { get; set; } = default!;
}
