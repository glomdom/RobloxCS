#nullable enable
namespace RobloxCS.Types;
[RobloxNative("FluidForceSensor", RobloxNativeType.Instance)]
public partial class FluidForceSensor : SensorBase  {
    public Vector3 CenterOfPressure { get; } = default!;
    public Vector3 Force { get; } = default!;
    public Vector3 Torque { get; } = default!;
    public LuaTuple EvaluateAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
