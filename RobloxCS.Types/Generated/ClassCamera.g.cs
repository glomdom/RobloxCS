#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Camera", RobloxNativeType.Instance)]
public partial class Camera : PVInstance  {
    public CFrame CFrame { get; } = default!;
    public Instance CameraSubject { get; } = default!;
    public Enums.CameraType CameraType { get; } = default!;
    public float DiagonalFieldOfView { get; } = default!;
    public float FieldOfView { get; } = default!;
    public Enums.FieldOfViewMode FieldOfViewMode { get; } = default!;
    public CFrame Focus { get; } = default!;
    public bool HeadLocked { get; } = default!;
    public float HeadScale { get; } = default!;
    public float MaxAxisFieldOfView { get; } = default!;
    public float NearPlaneZ { get; } = default!;
    public bool VRTiltAndRollEnabled { get; } = default!;
    public Vector2 ViewportSize { get; } = default!;
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
