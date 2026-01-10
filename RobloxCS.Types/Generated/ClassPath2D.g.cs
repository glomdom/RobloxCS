#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Path2D", RobloxNativeType.Instance)]
public partial class Path2D : GuiBase  {
    public bool Closed { get; set; } = default!;
    public Color3 Color3 { get; set; } = default!;
    public float Thickness { get; set; } = default!;
    public bool Visible { get; set; } = default!;
    public int ZIndex { get; set; } = default!;
    public Rect GetBoundingRect() => ThrowHelper.ThrowTranspiledMethod();
    public Path2DControlPoint GetControlPoint() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetControlPoints() => ThrowHelper.ThrowTranspiledMethod();
    public float GetLength() => ThrowHelper.ThrowTranspiledMethod();
    public int GetMaxControlPoints() => ThrowHelper.ThrowTranspiledMethod();
    public UDim2 GetPositionOnCurve() => ThrowHelper.ThrowTranspiledMethod();
    public UDim2 GetPositionOnCurveArcLength() => ThrowHelper.ThrowTranspiledMethod();
    public Vector2 GetTangentOnCurve() => ThrowHelper.ThrowTranspiledMethod();
    public Vector2 GetTangentOnCurveArcLength() => ThrowHelper.ThrowTranspiledMethod();
    public void InsertControlPoint() => ThrowHelper.ThrowTranspiledMethod();
    public void RemoveControlPoint() => ThrowHelper.ThrowTranspiledMethod();
    public void SetControlPoints() => ThrowHelper.ThrowTranspiledMethod();
    public void UpdateControlPoint() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal ControlPointChanged { get; private set; } = null!;
}
