#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VideoDisplay", RobloxNativeType.Instance)]
public partial class VideoDisplay : GuiObject  {
    public Enums.ResamplerMode ResampleMode { get; set; } = default!;
    public Enums.ScaleType ScaleType { get; set; } = default!;
    public UDim2 TileSize { get; set; } = default!;
    public Color3 VideoColor3 { get; set; } = default!;
    public Vector2 VideoRectOffset { get; set; } = default!;
    public Vector2 VideoRectSize { get; set; } = default!;
    public float VideoTransparency { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
