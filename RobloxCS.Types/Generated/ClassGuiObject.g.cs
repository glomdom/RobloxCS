#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiObject", RobloxNativeType.Instance)]
public partial class GuiObject : GuiBase2d  {
    public bool Active { get; } = default!;
    public Vector2 AnchorPoint { get; } = default!;
    public Enums.AutomaticSize AutomaticSize { get; } = default!;
    public Color3 BackgroundColor3 { get; } = default!;
    public float BackgroundTransparency { get; } = default!;
    public Color3 BorderColor3 { get; } = default!;
    public Enums.BorderMode BorderMode { get; } = default!;
    public int BorderSizePixel { get; } = default!;
    public bool ClipsDescendants { get; } = default!;
    public Enums.GuiState GuiState { get; } = default!;
    public bool Interactable { get; } = default!;
    public int LayoutOrder { get; } = default!;
    public GuiObject NextSelectionDown { get; } = default!;
    public GuiObject NextSelectionLeft { get; } = default!;
    public GuiObject NextSelectionRight { get; } = default!;
    public GuiObject NextSelectionUp { get; } = default!;
    public UDim2 Position { get; } = default!;
    public float Rotation { get; } = default!;
    public bool Selectable { get; } = default!;
    public GuiObject SelectionImageObject { get; } = default!;
    public int SelectionOrder { get; } = default!;
    public UDim2 Size { get; } = default!;
    public Enums.SizeConstraint SizeConstraint { get; } = default!;
    public bool Visible { get; } = default!;
    public int ZIndex { get; } = default!;
    public bool TweenPosition() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool TweenSize() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool TweenSizeAndPosition() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
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
