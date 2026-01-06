#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioGate", RobloxNativeType.Instance)]
public partial class AudioGate : Instance  {
    public float Attack { get; set; } = default!;
    public bool Bypass { get; set; } = default!;
    public float Release { get; set; } = default!;
    public NumberRange Threshold { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
