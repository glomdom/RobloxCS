#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Trail", RobloxNativeType.Instance)]
public partial class Trail : Instance  {
    public Attachment Attachment0 { get; } = default!;
    public Attachment Attachment1 { get; } = default!;
    public float Brightness { get; } = default!;
    public ColorSequence Color { get; } = default!;
    public bool Enabled { get; } = default!;
    public bool FaceCamera { get; } = default!;
    public float Lifetime { get; } = default!;
    public float LightEmission { get; } = default!;
    public float LightInfluence { get; } = default!;
    public float MaxLength { get; } = default!;
    public float MinLength { get; } = default!;
    public string Texture { get; } = default!;
    public float TextureLength { get; } = default!;
    public Enums.TextureMode TextureMode { get; } = default!;
    public NumberSequence Transparency { get; } = default!;
    public NumberSequence WidthScale { get; } = default!;
    public void Clear() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
