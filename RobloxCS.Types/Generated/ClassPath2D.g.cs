#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Path2D", RobloxNativeType.Instance)]
public partial class Path2D : GuiBase  {
    public bool Closed { get; set; } = default!;
    public Color3 Color3 { get; set; } = default!;
    public float Thickness { get; set; } = default!;
    public bool Visible { get; set; } = default!;
    public int ZIndex { get; set; } = default!;
    public Rect GetBoundingRect() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Path2DControlPoint GetControlPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetControlPoints() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetLength() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int GetMaxControlPoints() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public UDim2 GetPositionOnCurve() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public UDim2 GetPositionOnCurveArcLength() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Vector2 GetTangentOnCurve() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Vector2 GetTangentOnCurveArcLength() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void InsertControlPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveControlPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetControlPoints() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void UpdateControlPoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal ControlPointChanged { get; private set; } = null!;
}
