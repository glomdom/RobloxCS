#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Highlight", RobloxNativeType.Instance)]
public partial class Highlight : Instance  {
    public Instance Adornee { get; } = default!;
    public Enums.HighlightDepthMode DepthMode { get; } = default!;
    public bool Enabled { get; } = default!;
    public Color3 FillColor { get; } = default!;
    public float FillTransparency { get; } = default!;
    public Color3 OutlineColor { get; } = default!;
    public float OutlineTransparency { get; } = default!;
}
