#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIDragDetector", RobloxNativeType.Instance)]
public partial class UIDragDetector : UIComponent  {
    public string ActivatedCursorIcon { get; set; } = default!;
    public string ActivatedCursorIconContent { get; set; } = default!;
    public Enums.UIDragDetectorBoundingBehavior BoundingBehavior { get; set; } = default!;
    public GuiBase2d BoundingUI { get; set; } = default!;
    public string CursorIcon { get; set; } = default!;
    public string CursorIconContent { get; set; } = default!;
    public Vector2 DragAxis { get; set; } = default!;
    public Enums.UIDragDetectorDragRelativity DragRelativity { get; set; } = default!;
    public float DragRotation { get; set; } = default!;
    public Enums.UIDragDetectorDragSpace DragSpace { get; set; } = default!;
    public Enums.UIDragDetectorDragStyle DragStyle { get; set; } = default!;
    public UDim2 DragUDim2 { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public float MaxDragAngle { get; set; } = default!;
    public UDim2 MaxDragTranslation { get; set; } = default!;
    public float MinDragAngle { get; set; } = default!;
    public UDim2 MinDragTranslation { get; set; } = default!;
    public GuiObject ReferenceUIInstance { get; set; } = default!;
    public Enums.UIDragDetectorResponseStyle ResponseStyle { get; set; } = default!;
    public UDim2 SelectionModeDragSpeed { get; set; } = default!;
    public float SelectionModeRotateSpeed { get; set; } = default!;
    public Enums.UIDragSpeedAxisMapping UIDragSpeedAxisMapping { get; set; } = default!;
    public RBXScriptConnection AddConstraintFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public UDim2 GetReferencePosition() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetReferenceRotation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetDragStyleFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Vector2> DragContinue { get; private set; } = null!;
    public RBXScriptSignal<Vector2> DragEnd { get; private set; } = null!;
    public RBXScriptSignal<Vector2> DragStart { get; private set; } = null!;
}
