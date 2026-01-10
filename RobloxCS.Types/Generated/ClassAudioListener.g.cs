#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioListener", RobloxNativeType.Instance)]
public partial class AudioListener : Instance  {
    public bool AcousticSimulationEnabled { get; set; } = default!;
    public string AudioInteractionGroup { get; set; } = default!;
    public Dictionary<string, object> GetAngleAttenuation() => ThrowHelper.ThrowTranspiledMethod();
    public float GetAudibilityFor() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public Dictionary<string, object> GetDistanceAttenuation() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetInteractingEmitters() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public void SetAngleAttenuation() => ThrowHelper.ThrowTranspiledMethod();
    public void SetDistanceAttenuation() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
