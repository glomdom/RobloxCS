#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AtmosphereSensor", RobloxNativeType.Instance)]
public partial class AtmosphereSensor : SensorBase  {
    public float AirDensity { get; } = default!;
    public Vector3 RelativeWindVelocity { get; } = default!;
}
