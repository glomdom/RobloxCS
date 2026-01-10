#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerDataRecord", RobloxNativeType.Instance)]
public partial class PlayerDataRecord : Instance  {
    public int CreatedTime { get; set; } = default!;
    public bool DefaultRecordName { get; set; } = default!;
    public bool Dirty { get; set; } = default!;
    public Enums.PlayerDataErrorState Error { get; set; } = default!;
    public int FlushedTime { get; set; } = default!;
    public int LoadedTime { get; set; } = default!;
    public int ModifiedTime { get; set; } = default!;
    public bool NewRecord { get; set; } = default!;
    public bool Readable { get; set; } = default!;
    public string RecordName { get; set; } = default!;
    public bool Writable { get; set; } = default!;
    public Player GetPlayer() => ThrowHelper.ThrowTranspiledMethod();
    public object GetValue() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal GetValueChangedSignal() => ThrowHelper.ThrowTranspiledMethod();
    public void RemoveValue() => ThrowHelper.ThrowTranspiledMethod();
    public void SetValue() => ThrowHelper.ThrowTranspiledMethod();
    public void ReleaseAsync() => ThrowHelper.ThrowTranspiledMethod();
    public void RequestFlushAsync() => ThrowHelper.ThrowTranspiledMethod();
    new public RBXScriptSignal<string, object> Changed { get; private set; } = null!;
    public RBXScriptSignal<bool, string?> Flushed { get; private set; } = null!;
    public RBXScriptSignal<bool, string?> Loaded { get; private set; } = null!;
}
