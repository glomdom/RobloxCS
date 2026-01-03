#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PyramidHandleAdornment", RobloxNativeType.Instance)]
public partial class PyramidHandleAdornment : HandleAdornment  {
    public float Height { get; } = default!;
    public Enums.AdornShading Shading { get; } = default!;
    public int Sides { get; } = default!;
    public float Size { get; } = default!;
}
