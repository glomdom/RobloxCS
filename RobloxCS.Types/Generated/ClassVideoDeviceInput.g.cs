#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoDeviceInput", RobloxNativeType.Instance)]
public partial class VideoDeviceInput : Instance  {
    public bool Active { get; set; } = default!;
    public string CameraId { get; set; } = default!;
    public Enums.VideoDeviceCaptureQuality CaptureQuality { get; set; } = default!;
    public bool IsReady { get; set; } = default!;
}
