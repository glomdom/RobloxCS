#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIStroke", RobloxNativeType.Instance)]
public partial class UIStroke : UIComponent  {
    public Enums.ApplyStrokeMode ApplyStrokeMode { get; } = default!;
    public UDim BorderOffset { get; } = default!;
    public Enums.BorderStrokePosition BorderStrokePosition { get; } = default!;
    public Color3 Color { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.LineJoinMode LineJoinMode { get; } = default!;
    public Enums.StrokeSizingMode StrokeSizingMode { get; } = default!;
    public float Thickness { get; } = default!;
    public float Transparency { get; } = default!;
    public int ZIndex { get; } = default!;
}
