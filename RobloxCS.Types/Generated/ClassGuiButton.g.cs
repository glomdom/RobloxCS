#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiButton", RobloxNativeType.Instance)]
public partial class GuiButton : GuiObject  {
    public bool AutoButtonColor { get; set; } = default!;
    public HapticEffect HoverHapticEffect { get; set; } = default!;
    public bool Modal { get; set; } = default!;
    public HapticEffect PressHapticEffect { get; set; } = default!;
    public bool Selected { get; set; } = default!;
    public Enums.ButtonStyle Style { get; set; } = default!;
    public RBXScriptSignal<InputObject, int> Activated { get; private set; } = null!;
    public RBXScriptSignal MouseButton1Click { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton1Down { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton1Up { get; private set; } = null!;
    public RBXScriptSignal MouseButton2Click { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton2Down { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseButton2Up { get; private set; } = null!;
    public RBXScriptSignal<InputObject> SecondaryActivated { get; private set; } = null!;
}
