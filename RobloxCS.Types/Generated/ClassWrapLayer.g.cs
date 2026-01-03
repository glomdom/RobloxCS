#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WrapLayer", RobloxNativeType.Instance)]
public partial class WrapLayer : BaseWrap  {
    public Enums.WrapLayerAutoSkin AutoSkin { get; } = default!;
    public CFrame BindOffset { get; } = default!;
    public Color3 Color { get; } = default!;
    public Enums.WrapLayerDebugMode DebugMode { get; } = default!;
    public bool Enabled { get; } = default!;
    public int Order { get; } = default!;
    public float Puffiness { get; } = default!;
    public string ReferenceMeshContent { get; } = default!;
    public string ReferenceMeshId { get; } = default!;
    public CFrame ReferenceOrigin { get; } = default!;
    public CFrame ReferenceOriginWorld { get; } = default!;
    public float ShrinkFactor { get; } = default!;
}
