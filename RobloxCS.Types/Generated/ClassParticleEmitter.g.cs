#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ParticleEmitter", RobloxNativeType.Instance)]
public partial class ParticleEmitter : Instance  {
    public Vector3 Acceleration { get; set; } = default!;
    public float Brightness { get; set; } = default!;
    public ColorSequence Color { get; set; } = default!;
    public float Drag { get; set; } = default!;
    public Enums.NormalId EmissionDirection { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public NumberRange FlipbookFramerate { get; set; } = default!;
    public string FlipbookIncompatible { get; set; } = default!;
    public Enums.ParticleFlipbookLayout FlipbookLayout { get; set; } = default!;
    public Enums.ParticleFlipbookMode FlipbookMode { get; set; } = default!;
    public int FlipbookSizeX { get; set; } = default!;
    public int FlipbookSizeY { get; set; } = default!;
    public bool FlipbookStartRandom { get; set; } = default!;
    public NumberRange Lifetime { get; set; } = default!;
    public float LightEmission { get; set; } = default!;
    public float LightInfluence { get; set; } = default!;
    public bool LockedToPart { get; set; } = default!;
    public Enums.ParticleOrientation Orientation { get; set; } = default!;
    public float Rate { get; set; } = default!;
    public NumberRange RotSpeed { get; set; } = default!;
    public NumberRange Rotation { get; set; } = default!;
    public Enums.ParticleEmitterShape Shape { get; set; } = default!;
    public Enums.ParticleEmitterShapeInOut ShapeInOut { get; set; } = default!;
    public float ShapePartial { get; set; } = default!;
    public Enums.ParticleEmitterShapeStyle ShapeStyle { get; set; } = default!;
    public NumberSequence Size { get; set; } = default!;
    public NumberRange Speed { get; set; } = default!;
    public Vector2 SpreadAngle { get; set; } = default!;
    public NumberSequence Squash { get; set; } = default!;
    public string Texture { get; set; } = default!;
    public float TimeScale { get; set; } = default!;
    public NumberSequence Transparency { get; set; } = default!;
    public float VelocityInheritance { get; set; } = default!;
    public bool WindAffectsDrag { get; set; } = default!;
    public float ZOffset { get; set; } = default!;
    public void Clear() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Emit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
