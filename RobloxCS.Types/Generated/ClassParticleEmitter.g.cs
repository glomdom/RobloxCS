#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ParticleEmitter", RobloxNativeType.Instance)]
public partial class ParticleEmitter : Instance  {
    public Vector3 Acceleration { get; } = default!;
    public float Brightness { get; } = default!;
    public ColorSequence Color { get; } = default!;
    public float Drag { get; } = default!;
    public Enums.NormalId EmissionDirection { get; } = default!;
    public bool Enabled { get; } = default!;
    public NumberRange FlipbookFramerate { get; } = default!;
    public string FlipbookIncompatible { get; } = default!;
    public Enums.ParticleFlipbookLayout FlipbookLayout { get; } = default!;
    public Enums.ParticleFlipbookMode FlipbookMode { get; } = default!;
    public int FlipbookSizeX { get; } = default!;
    public int FlipbookSizeY { get; } = default!;
    public bool FlipbookStartRandom { get; } = default!;
    public NumberRange Lifetime { get; } = default!;
    public float LightEmission { get; } = default!;
    public float LightInfluence { get; } = default!;
    public bool LockedToPart { get; } = default!;
    public Enums.ParticleOrientation Orientation { get; } = default!;
    public float Rate { get; } = default!;
    public NumberRange RotSpeed { get; } = default!;
    public NumberRange Rotation { get; } = default!;
    public Enums.ParticleEmitterShape Shape { get; } = default!;
    public Enums.ParticleEmitterShapeInOut ShapeInOut { get; } = default!;
    public float ShapePartial { get; } = default!;
    public Enums.ParticleEmitterShapeStyle ShapeStyle { get; } = default!;
    public NumberSequence Size { get; } = default!;
    public NumberRange Speed { get; } = default!;
    public Vector2 SpreadAngle { get; } = default!;
    public NumberSequence Squash { get; } = default!;
    public string Texture { get; } = default!;
    public float TimeScale { get; } = default!;
    public NumberSequence Transparency { get; } = default!;
    public float VelocityInheritance { get; } = default!;
    public bool WindAffectsDrag { get; } = default!;
    public float ZOffset { get; } = default!;
    public void Clear() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Emit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
