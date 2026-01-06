#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioEmitter", RobloxNativeType.Instance)]
public partial class AudioEmitter : Instance  {
    public bool AcousticSimulationEnabled { get; set; } = default!;
    public string AudioInteractionGroup { get; set; } = default!;
    public Dictionary<string, object> GetAngleAttenuation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float GetAudibilityFor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<string, object> GetDistanceAttenuation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetInteractingListeners() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetAngleAttenuation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetDistanceAttenuation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
