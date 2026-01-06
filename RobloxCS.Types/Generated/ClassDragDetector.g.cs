#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DragDetector", RobloxNativeType.Instance)]
public partial class DragDetector : ClickDetector  {
    public string ActivatedCursorIcon { get; set; } = default!;
    public string ActivatedCursorIconContent { get; set; } = default!;
    public bool ApplyAtCenterOfMass { get; set; } = default!;
    public Vector3 Axis { get; set; } = default!;
    public CFrame DragFrame { get; set; } = default!;
    public Enums.DragDetectorDragStyle DragStyle { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public Enums.KeyCode GamepadModeSwitchKeyCode { get; set; } = default!;
    public Enums.KeyCode KeyboardModeSwitchKeyCode { get; set; } = default!;
    public float MaxDragAngle { get; set; } = default!;
    public Vector3 MaxDragTranslation { get; set; } = default!;
    public float MaxForce { get; set; } = default!;
    public float MaxTorque { get; set; } = default!;
    public float MinDragAngle { get; set; } = default!;
    public Vector3 MinDragTranslation { get; set; } = default!;
    public Vector3 Orientation { get; set; } = default!;
    public Enums.DragDetectorPermissionPolicy PermissionPolicy { get; set; } = default!;
    public Instance ReferenceInstance { get; set; } = default!;
    public Enums.DragDetectorResponseStyle ResponseStyle { get; set; } = default!;
    public float Responsiveness { get; set; } = default!;
    public bool RunLocally { get; set; } = default!;
    public Vector3 SecondaryAxis { get; set; } = default!;
    public float TrackballRadialPullFactor { get; set; } = default!;
    public float TrackballRollFactor { get; set; } = default!;
    public Enums.KeyCode VRSwitchKeyCode { get; set; } = default!;
    public Vector3 WorldAxis { get; set; } = default!;
    public Vector3 WorldSecondaryAxis { get; set; } = default!;
    public RBXScriptConnection AddConstraintFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetReferenceFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RestartDrag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetDragStyleFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetPermissionPolicyFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Player, Ray, CFrame, CFrame?, bool> DragContinue { get; private set; } = null!;
    public RBXScriptSignal<Player> DragEnd { get; private set; } = null!;
    public RBXScriptSignal<Player, Ray, CFrame, CFrame, BasePart, CFrame?, bool> DragStart { get; private set; } = null!;
}
