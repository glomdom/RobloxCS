#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioDeviceOutput", RobloxNativeType.Instance)]
public partial class AudioDeviceOutput : Instance  {
    public Player Player { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
