#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WireframeHandleAdornment", RobloxNativeType.Instance)]
public partial class WireframeHandleAdornment : HandleAdornment  {
    public Vector3 Scale { get; set; } = default!;
    public float Thickness { get; set; } = default!;
    public void AddLine() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void AddLines() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void AddPath() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void AddText() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Clear() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
