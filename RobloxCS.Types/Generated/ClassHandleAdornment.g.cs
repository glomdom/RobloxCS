#nullable enable
namespace RobloxCS.Types;
[RobloxNative("HandleAdornment", RobloxNativeType.Instance)]
public partial class HandleAdornment : PVAdornment  {
    public Enums.AdornCullingMode AdornCullingMode { get; } = default!;
    public bool AlwaysOnTop { get; } = default!;
    public CFrame CFrame { get; } = default!;
    public Vector3 SizeRelativeOffset { get; } = default!;
    public int ZIndex { get; } = default!;
    public RBXScriptSignal MouseButton1Down { get; private set; } = null!;
    public RBXScriptSignal MouseButton1Up { get; private set; } = null!;
    public RBXScriptSignal MouseEnter { get; private set; } = null!;
    public RBXScriptSignal MouseLeave { get; private set; } = null!;
}
