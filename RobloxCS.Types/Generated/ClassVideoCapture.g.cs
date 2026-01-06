#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoCapture", RobloxNativeType.Instance)]
public partial class VideoCapture : Capture  {
    public double TimeLength { get; set; } = default!;
}
