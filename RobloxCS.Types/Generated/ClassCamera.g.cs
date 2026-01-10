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
    public List<Instance> GetPartsObscuringTarget() => ThrowHelper.ThrowTranspiledMethod();
    public CFrame GetRenderCFrame() => ThrowHelper.ThrowTranspiledMethod();
    public float GetRoll() => ThrowHelper.ThrowTranspiledMethod();
    public Ray ScreenPointToRay() => ThrowHelper.ThrowTranspiledMethod();
    public void SetRoll() => ThrowHelper.ThrowTranspiledMethod();
    public Ray ViewportPointToRay() => ThrowHelper.ThrowTranspiledMethod();
    public LuaTuple WorldToScreenPoint() => ThrowHelper.ThrowTranspiledMethod();
    public LuaTuple WorldToViewportPoint() => ThrowHelper.ThrowTranspiledMethod();
    public void ZoomToExtents() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal InterpolationFinished { get; private set; } = null!;
}
