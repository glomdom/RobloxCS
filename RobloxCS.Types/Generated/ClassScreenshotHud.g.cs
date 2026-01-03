#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ScreenshotHud", RobloxNativeType.Instance)]
public partial class ScreenshotHud : Instance  {
    public string CameraButtonIcon { get; } = default!;
    public UDim2 CameraButtonPosition { get; } = default!;
    public UDim2 CloseButtonPosition { get; } = default!;
    public bool CloseWhenScreenshotTaken { get; } = default!;
    public bool HideCoreGuiForCaptures { get; } = default!;
    public bool HidePlayerGuiForCaptures { get; } = default!;
    public bool Visible { get; } = default!;
}
