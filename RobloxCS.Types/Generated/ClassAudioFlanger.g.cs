#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioFlanger", RobloxNativeType.Instance)]
public partial class AudioFlanger : Instance  {
    public bool Bypass { get; } = default!;
    public float Depth { get; } = default!;
    public float Mix { get; } = default!;
    public float Rate { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
