#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoDeviceInput", RobloxNativeType.Instance)]
public partial class VideoDeviceInput : Instance  {
    public bool Active { get; } = default!;
    public string CameraId { get; } = default!;
    public Enums.VideoDeviceCaptureQuality CaptureQuality { get; } = default!;
    public bool IsReady { get; } = default!;
}
