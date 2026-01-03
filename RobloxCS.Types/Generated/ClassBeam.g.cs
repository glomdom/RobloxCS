#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Beam", RobloxNativeType.Instance)]
public partial class Beam : Instance  {
    public Attachment Attachment0 { get; } = default!;
    public Attachment Attachment1 { get; } = default!;
    public float Brightness { get; } = default!;
    public ColorSequence Color { get; } = default!;
    public float CurveSize0 { get; } = default!;
    public float CurveSize1 { get; } = default!;
    public bool Enabled { get; } = default!;
    public bool FaceCamera { get; } = default!;
    public float LightEmission { get; } = default!;
    public float LightInfluence { get; } = default!;
    public int Segments { get; } = default!;
    public string Texture { get; } = default!;
    public float TextureLength { get; } = default!;
    public Enums.TextureMode TextureMode { get; } = default!;
    public float TextureSpeed { get; } = default!;
    public NumberSequence Transparency { get; } = default!;
    public float Width0 { get; } = default!;
    public float Width1 { get; } = default!;
    public float ZOffset { get; } = default!;
    public void SetTextureOffset() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
