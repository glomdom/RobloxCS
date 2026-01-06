#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ImageHandleAdornment", RobloxNativeType.Instance)]
public partial class ImageHandleAdornment : HandleAdornment  {
    public string Image { get; set; } = default!;
    public Vector2 Size { get; set; } = default!;
}
