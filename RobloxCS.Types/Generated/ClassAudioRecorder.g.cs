#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioRecorder", RobloxNativeType.Instance)]
public partial class AudioRecorder : Instance  {
    public bool IsRecording { get; } = default!;
    public double TimeLength { get; set; } = default!;
    public void Clear() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetConnectedWires() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetInputPins() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetOutputPins() => ThrowHelper.ThrowTranspiledMethod();
    public string GetTemporaryContent() => ThrowHelper.ThrowTranspiledMethod();
    public void Stop() => ThrowHelper.ThrowTranspiledMethod();
    public bool CanRecordAsync() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetUnrecordableInstancesAsync() => ThrowHelper.ThrowTranspiledMethod();
    public void RecordAsync() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<bool, string, Wire, Instance> WiringChanged { get; private set; } = null!;
}
