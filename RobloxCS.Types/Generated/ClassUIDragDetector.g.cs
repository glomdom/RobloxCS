#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIDragDetector", RobloxNativeType.Instance)]
public partial class UIDragDetector : UIComponent  {
    public string ActivatedCursorIcon { get; } = default!;
    public string ActivatedCursorIconContent { get; } = default!;
    public Enums.UIDragDetectorBoundingBehavior BoundingBehavior { get; } = default!;
    public GuiBase2d BoundingUI { get; } = default!;
    public string CursorIcon { get; } = default!;
    public string CursorIconContent { get; } = default!;
    public Vector2 DragAxis { get; } = default!;
    public Enums.UIDragDetectorDragRelativity DragRelativity { get; } = default!;
    public float DragRotation { get; } = default!;
    public Enums.UIDragDetectorDragSpace DragSpace { get; } = default!;
    public Enums.UIDragDetectorDragStyle DragStyle { get; } = default!;
    public UDim2 DragUDim2 { get; } = default!;
    public bool Enabled { get; } = default!;
    public float MaxDragAngle { get; } = default!;
    public UDim2 MaxDragTranslation { get; } = default!;
    public float MinDragAngle { get; } = default!;
    public UDim2 MinDragTranslation { get; } = default!;
    public GuiObject ReferenceUIInstance { get; } = default!;
    public Enums.UIDragDetectorResponseStyle ResponseStyle { get; } = default!;
    public UDim2 SelectionModeDragSpeed { get; } = default!;
    public float SelectionModeRotateSpeed { get; } = default!;
    public Enums.UIDragSpeedAxisMapping UIDragSpeedAxisMapping { get; } = default!;
    public RBXScriptConnection AddConstraintFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public UDim2 GetReferencePosition() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetReferenceRotation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetDragStyleFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Vector2> DragContinue { get; private set; } = null!;
    public RBXScriptSignal<Vector2> DragEnd { get; private set; } = null!;
    public RBXScriptSignal<Vector2> DragStart { get; private set; } = null!;
}
