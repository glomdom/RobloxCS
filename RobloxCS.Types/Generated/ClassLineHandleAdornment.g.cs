#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LineHandleAdornment", RobloxNativeType.Instance)]
public partial class LineHandleAdornment : HandleAdornment  {
    public float Length { get; set; } = default!;
    public float Thickness { get; set; } = default!;
}
