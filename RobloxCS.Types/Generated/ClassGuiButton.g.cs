#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiButton", RobloxNativeType.Instance)]
public partial class GuiButton : GuiObject  {
    public bool AutoButtonColor { get; } = default!;
    public HapticEffect HoverHapticEffect { get; } = default!;
    public bool Modal { get; } = default!;
    public HapticEffect PressHapticEffect { get; } = default!;
    public bool Selected { get; } = default!;
    public Enums.ButtonStyle Style { get; } = default!;
    public RBXScriptSignal<InputObject, int> Activated { get; private set; } = null!;
    public RBXScriptSignal MouseButton1Click { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton1Down { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton1Up { get; private set; } = null!;
    public RBXScriptSignal MouseButton2Click { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton2Down { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton2Up { get; private set; } = null!;
    public RBXScriptSignal<InputObject> SecondaryActivated { get; private set; } = null!;
}
