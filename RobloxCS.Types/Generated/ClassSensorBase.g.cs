#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SensorBase", RobloxNativeType.Instance)]
public partial class SensorBase : Instance  {
    public Enums.SensorUpdateType UpdateType { get; set; } = default!;
    public RBXScriptSignal OnSensorOutputChanged { get; private set; } = null!;
}
