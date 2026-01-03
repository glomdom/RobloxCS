#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Decal", RobloxNativeType.Instance)]
public partial class Decal : FaceInstance  {
    public Color3 Color3 { get; } = default!;
    public string ColorMap { get; } = default!;
    public string ColorMapContent { get; } = default!;
    public string MetalnessMapContent { get; } = default!;
    public string NormalMapContent { get; } = default!;
    public string RoughnessMapContent { get; } = default!;
    public string Texture { get; } = default!;
    public string TextureContent { get; } = default!;
    public float Transparency { get; } = default!;
    public Vector2 UVOffset { get; } = default!;
    public Vector2 UVScale { get; } = default!;
    public int ZIndex { get; } = default!;
}
