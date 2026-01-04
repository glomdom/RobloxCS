#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerDataRecord", RobloxNativeType.Instance)]
public partial class PlayerDataRecord : Instance  {
    public int CreatedTime { get; } = default!;
    public bool DefaultRecordName { get; } = default!;
    public bool Dirty { get; } = default!;
    public Enums.PlayerDataErrorState Error { get; } = default!;
    public int FlushedTime { get; } = default!;
    public int LoadedTime { get; } = default!;
    public int ModifiedTime { get; } = default!;
    public bool NewRecord { get; } = default!;
    public bool Readable { get; } = default!;
    public string RecordName { get; } = default!;
    public bool Writable { get; } = default!;
    public Player GetPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal GetValueChangedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RemoveValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ReleaseAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RequestFlushAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    new public RBXScriptSignal<string, object> Changed { get; private set; } = null!;
    public RBXScriptSignal<bool, string?> Flushed { get; private set; } = null!;
    public RBXScriptSignal<bool, string?> Loaded { get; private set; } = null!;
}
