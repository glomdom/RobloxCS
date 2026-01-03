#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VehicleSeat", RobloxNativeType.Instance)]
public partial class VehicleSeat : BasePart  {
    public int AreHingesDetected { get; } = default!;
    public bool Disabled { get; } = default!;
    public bool HeadsUpDisplay { get; } = default!;
    public float MaxSpeed { get; } = default!;
    public Humanoid Occupant { get; } = default!;
    public int Steer { get; } = default!;
    public float SteerFloat { get; } = default!;
    public int Throttle { get; } = default!;
    public float ThrottleFloat { get; } = default!;
    public float Torque { get; } = default!;
    public float TurnSpeed { get; } = default!;
    public void Sit() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
