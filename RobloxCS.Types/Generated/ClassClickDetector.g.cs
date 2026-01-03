#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ClickDetector", RobloxNativeType.Instance)]
public partial class ClickDetector : Instance  {
    public string CursorIcon { get; } = default!;
    public string CursorIconContent { get; } = default!;
    public float MaxActivationDistance { get; } = default!;
}
