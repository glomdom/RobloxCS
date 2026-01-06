#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioChorus", RobloxNativeType.Instance)]
public partial class AudioChorus : Instance  {
    public bool Bypass { get; set; } = default!;
    public float Depth { get; set; } = default!;
    public float Mix { get; set; } = default!;
    public float Rate { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
