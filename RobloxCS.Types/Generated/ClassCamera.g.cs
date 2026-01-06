#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Camera", RobloxNativeType.Instance)]
public partial class Camera : PVInstance  {
    public CFrame CFrame { get; set; } = default!;
    public Instance CameraSubject { get; set; } = default!;
    public Enums.CameraType CameraType { get; set; } = default!;
    public float DiagonalFieldOfView { get; set; } = default!;
    public float FieldOfView { get; set; } = default!;
    public Enums.FieldOfViewMode FieldOfViewMode { get; set; } = default!;
    public CFrame Focus { get; set; } = default!;
    public bool HeadLocked { get; set; } = default!;
    public float HeadScale { get; set; } = default!;
    public float MaxAxisFieldOfView { get; set; } = default!;
    public float NearPlaneZ { get; set; } = default!;
    public bool VRTiltAndRollEnabled { get; set; } = default!;
    public Vector2 ViewportSize { get; set; } = default!;
    public List<Instance> GetPartsObscuringTarget() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetRenderCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetRoll() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Ray ScreenPointToRay() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetRoll() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Ray ViewportPointToRay() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple WorldToScreenPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple WorldToViewportPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ZoomToExtents() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal InterpolationFinished { get; private set; } = null!;
}
