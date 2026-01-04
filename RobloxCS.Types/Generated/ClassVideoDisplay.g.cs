#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoDisplay", RobloxNativeType.Instance)]
public partial class VideoDisplay : GuiObject  {
    public Enums.ResamplerMode ResampleMode { get; } = default!;
    public Enums.ScaleType ScaleType { get; } = default!;
    public UDim2 TileSize { get; } = default!;
    public Color3 VideoColor3 { get; } = default!;
    public Vector2 VideoRectOffset { get; } = default!;
    public Vector2 VideoRectSize { get; } = default!;
    public float VideoTransparency { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
