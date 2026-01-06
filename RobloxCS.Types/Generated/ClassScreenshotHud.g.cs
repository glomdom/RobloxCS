#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ScreenshotHud", RobloxNativeType.Instance)]
public partial class ScreenshotHud : Instance  {
    public string CameraButtonIcon { get; set; } = default!;
    public UDim2 CameraButtonPosition { get; set; } = default!;
    public UDim2 CloseButtonPosition { get; set; } = default!;
    public bool CloseWhenScreenshotTaken { get; set; } = default!;
    public bool HideCoreGuiForCaptures { get; set; } = default!;
    public bool HidePlayerGuiForCaptures { get; set; } = default!;
    public bool Visible { get; set; } = default!;
}
