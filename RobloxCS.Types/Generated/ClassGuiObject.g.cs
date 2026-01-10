#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiObject", RobloxNativeType.Instance)]
public partial class GuiObject : GuiBase2d  {
    public bool Active { get; set; } = default!;
    public Vector2 AnchorPoint { get; set; } = default!;
    public Enums.AutomaticSize AutomaticSize { get; set; } = default!;
    public Color3 BackgroundColor3 { get; set; } = default!;
    public float BackgroundTransparency { get; set; } = default!;
    public Color3 BorderColor3 { get; set; } = default!;
    public Enums.BorderMode BorderMode { get; set; } = default!;
    public int BorderSizePixel { get; set; } = default!;
    public bool ClipsDescendants { get; set; } = default!;
    public Enums.GuiState GuiState { get; set; } = default!;
    public bool Interactable { get; set; } = default!;
    public int LayoutOrder { get; set; } = default!;
    public GuiObject NextSelectionDown { get; set; } = default!;
    public GuiObject NextSelectionLeft { get; set; } = default!;
    public GuiObject NextSelectionRight { get; set; } = default!;
    public GuiObject NextSelectionUp { get; set; } = default!;
    public UDim2 Position { get; set; } = default!;
    public float Rotation { get; set; } = default!;
    public bool Selectable { get; set; } = default!;
    public GuiObject SelectionImageObject { get; set; } = default!;
    public int SelectionOrder { get; set; } = default!;
    public UDim2 Size { get; set; } = default!;
    public Enums.SizeConstraint SizeConstraint { get; set; } = default!;
    public bool Visible { get; set; } = default!;
    public int ZIndex { get; set; } = default!;
    public bool TweenPosition() => ThrowHelper.ThrowTranspiledMethod();
    public bool TweenSize() => ThrowHelper.ThrowTranspiledMethod();
    public bool TweenSizeAndPosition() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<InputObject> InputBegan { get; private set; } = null!;
    public RBXScriptSignal<InputObject> InputChanged { get; private set; } = null!;
    public RBXScriptSignal<InputObject> InputEnded { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseEnter { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseLeave { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseMoved { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseWheelBackward { get; private set; } = null!;
    public RBXScriptSignal<int, int> MouseWheelForward { get; private set; } = null!;
    public RBXScriptSignal SelectionGained { get; private set; } = null!;
    public RBXScriptSignal SelectionLost { get; private set; } = null!;
    public RBXScriptSignal<List<object>, Enums.UserInputState> TouchLongPress { get; private set; } = null!;
    public RBXScriptSignal<List<object>, Vector2, Vector2, Enums.UserInputState> TouchPan { get; private set; } = null!;
    public RBXScriptSignal<List<object>, float, float, Enums.UserInputState> TouchPinch { get; private set; } = null!;
    public RBXScriptSignal<List<object>, float, float, Enums.UserInputState> TouchRotate { get; private set; } = null!;
    public RBXScriptSignal<Enums.SwipeDirection, int> TouchSwipe { get; private set; } = null!;
    public RBXScriptSignal<List<object>> TouchTap { get; private set; } = null!;
}
