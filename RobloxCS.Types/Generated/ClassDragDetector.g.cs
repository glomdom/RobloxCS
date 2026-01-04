#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DragDetector", RobloxNativeType.Instance)]
public partial class DragDetector : ClickDetector  {
    public string ActivatedCursorIcon { get; } = default!;
    public string ActivatedCursorIconContent { get; } = default!;
    public bool ApplyAtCenterOfMass { get; } = default!;
    public Vector3 Axis { get; } = default!;
    public CFrame DragFrame { get; } = default!;
    public Enums.DragDetectorDragStyle DragStyle { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.KeyCode GamepadModeSwitchKeyCode { get; } = default!;
    public Enums.KeyCode KeyboardModeSwitchKeyCode { get; } = default!;
    public float MaxDragAngle { get; } = default!;
    public Vector3 MaxDragTranslation { get; } = default!;
    public float MaxForce { get; } = default!;
    public float MaxTorque { get; } = default!;
    public float MinDragAngle { get; } = default!;
    public Vector3 MinDragTranslation { get; } = default!;
    public Vector3 Orientation { get; } = default!;
    public Enums.DragDetectorPermissionPolicy PermissionPolicy { get; } = default!;
    public Instance ReferenceInstance { get; } = default!;
    public Enums.DragDetectorResponseStyle ResponseStyle { get; } = default!;
    public float Responsiveness { get; } = default!;
    public bool RunLocally { get; } = default!;
    public Vector3 SecondaryAxis { get; } = default!;
    public float TrackballRadialPullFactor { get; } = default!;
    public float TrackballRollFactor { get; } = default!;
    public Enums.KeyCode VRSwitchKeyCode { get; } = default!;
    public Vector3 WorldAxis { get; } = default!;
    public Vector3 WorldSecondaryAxis { get; } = default!;
    public RBXScriptConnection AddConstraintFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetReferenceFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RestartDrag() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetDragStyleFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetPermissionPolicyFunction() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Player, Ray, CFrame, CFrame?, bool> DragContinue { get; private set; } = null!;
    public RBXScriptSignal<Player> DragEnd { get; private set; } = null!;
    public RBXScriptSignal<Player, Ray, CFrame, CFrame, BasePart, CFrame?, bool> DragStart { get; private set; } = null!;
}
