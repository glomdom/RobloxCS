#nullable enable
namespace RobloxCS.Types;
[RobloxNative("HandleAdornment", RobloxNativeType.Instance)]
public partial class HandleAdornment : PVAdornment  {
    public Enums.AdornCullingMode AdornCullingMode { get; set; } = default!;
    public bool AlwaysOnTop { get; set; } = default!;
    public CFrame CFrame { get; set; } = default!;
    public Vector3 SizeRelativeOffset { get; set; } = default!;
    public int ZIndex { get; set; } = default!;
    public RBXScriptSignal MouseButton1Down { get; private set; } = null!;
    public RBXScriptSignal MouseButton1Up { get; private set; } = null!;
    public RBXScriptSignal MouseEnter { get; private set; } = null!;
    public RBXScriptSignal MouseLeave { get; private set; } = null!;
}
