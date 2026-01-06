#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioDeviceInput", RobloxNativeType.Instance)]
public partial class AudioDeviceInput : Instance  {
    public Enums.AccessModifierType AccessType { get; set; } = default!;
    public bool Active { get; } = default!;
    public bool Muted { get; set; } = default!;
    public Player Player { get; set; } = default!;
    public float Volume { get; set; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetUserIdAccessList() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetUserIdAccessList() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
