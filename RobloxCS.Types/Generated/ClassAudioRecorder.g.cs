#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioRecorder", RobloxNativeType.Instance)]
public partial class AudioRecorder : Instance  {
    public bool IsRecording { get; } = default!;
    public double TimeLength { get; set; } = default!;
    public void Clear() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetInputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetOutputPins() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public string GetTemporaryContent() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Stop() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool CanRecordAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetUnrecordableInstancesAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RecordAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
