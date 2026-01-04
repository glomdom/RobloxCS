#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ClickDetector", RobloxNativeType.Instance)]
public partial class ClickDetector : Instance  {
    public string CursorIcon { get; } = default!;
    public string CursorIconContent { get; } = default!;
    public float MaxActivationDistance { get; } = default!;
    public RBXScriptSignal<Player> MouseClick { get; private set; } = null!;
    public RBXScriptSignal<Player> MouseHoverEnter { get; private set; } = null!;
    public RBXScriptSignal<Player> MouseHoverLeave { get; private set; } = null!;
    public RBXScriptSignal<Player> RightMouseClick { get; private set; } = null!;
}
