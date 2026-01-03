#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BuoyancySensor", RobloxNativeType.Instance)]
public partial class BuoyancySensor : SensorBase  {
    public bool FullySubmerged { get; } = default!;
    public bool TouchingSurface { get; } = default!;
}
