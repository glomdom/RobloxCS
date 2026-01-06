#nullable enable
namespace RobloxCS.Types;
[RobloxNative("FluidForceSensor", RobloxNativeType.Instance)]
public partial class FluidForceSensor : SensorBase  {
    public Vector3 CenterOfPressure { get; set; } = default!;
    public Vector3 Force { get; set; } = default!;
    public Vector3 Torque { get; set; } = default!;
    public LuaTuple EvaluateAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
